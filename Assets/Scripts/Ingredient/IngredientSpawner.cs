using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class IngredientSpawner : MonoBehaviour {
    public List<IngredientData> m_AvailableIngredients;
    public static Action<Ingredient> OnIngredientCreate_Event;

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
        var ingredientSO = m_AvailableIngredients[UnityEngine.Random.Range(0, m_AvailableIngredients.Count)];
        Ingredient ingredient = new Ingredient(ingredientSO);
        OnIngredientCreate_Event?.Invoke( ingredient );
        return ingredient;
    }
}
