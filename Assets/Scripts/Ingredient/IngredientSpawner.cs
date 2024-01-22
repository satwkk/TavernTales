using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class IngredientSpawner : MonoBehaviour {
    // public List<IngredientData> m_AvailableIngredients;
    
    public List<Ingredient> m_AvailableIngredients;
    public static Action<Ingredient> OnIngredientCreate_Event;
    
    public Ingredient chosenIngredient;

    public static IngredientSpawner Instance { get; set; }

    private void Awake()
    {
        if ( Instance == null )
        {
            Instance = this;
        }
        else
        {
            Destroy( gameObject );
        }
    }

    public Ingredient CreateIngredient() {
        var randomIngredient = m_AvailableIngredients[UnityEngine.Random.Range(0, m_AvailableIngredients.Count)];
        // IngredientData ingData = new IngredientData(ingredientSO);
        chosenIngredient = Instantiate(randomIngredient);
        // chosenIngredient = new Ingredient(randomIngredient);
        OnIngredientCreate_Event?.Invoke( chosenIngredient );
        return chosenIngredient;
    }
}
