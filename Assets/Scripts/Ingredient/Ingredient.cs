using System;
using UnityEngine;

[Serializable]
public class Ingredient : MonoBehaviour
{
    [SerializeField] private IngredientData m_IngredientData;

    public IngredientData IngredientData 
    {
        get 
        {
            return m_IngredientData;
        }
        set 
        {
            m_IngredientData = value;
        }
    }
    
    public Action OnServe;
    [SerializeField] private bool isServed;
    public bool IsServed { 
        get => isServed; 
        set
        {
            isServed = value;
            if (isServed == true)
            {
                OnServe?.Invoke();
            }
        }
    }

    public Ingredient(IngredientData ingredientData) {
        m_IngredientData = ingredientData;
        isServed = false;
    }

    public Ingredient(Ingredient other) {
        this.m_IngredientData = other.m_IngredientData;
    }
}
