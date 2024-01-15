using System;
using UnityEngine;

[Serializable]
public class Ingredient {
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

    public Ingredient(IngredientData ingredientData) {
        m_IngredientData = ingredientData;
    }

    public Ingredient(Ingredient other) {
        this.m_IngredientData = other.m_IngredientData;
    }
}
