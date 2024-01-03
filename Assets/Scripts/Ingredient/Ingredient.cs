using System;
using UnityEngine;

[Serializable]
public class Ingredient {
    [SerializeField] private IngredientData m_IngredientData;

    public IngredientData IngredientData => m_IngredientData;

    public Ingredient(IngredientData ingredientData) {
        m_IngredientData = ingredientData;
    }
    
    public bool IsServed() { return m_IngredientData.isServed == true; }
}
