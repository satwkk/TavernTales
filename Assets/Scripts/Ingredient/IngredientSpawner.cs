using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class IngredientSpawner : MonoBehaviour {
    public List<IngredientData> m_AvailableIngredients;
    public static Action<Ingredient> OnIngredientCreate_Event;
    private Ingredient m_CurrentIngredient;
    public Ingredient CurrentIngredient {
        get {
            return m_CurrentIngredient;
        }
        set {
            m_CurrentIngredient = value;
            OnIngredientCreate_Event?.Invoke(m_CurrentIngredient);
        }
    }

    public Ingredient CreateIngredient() {
        var ingredientSO = m_AvailableIngredients[UnityEngine.Random.Range(0, m_AvailableIngredients.Count)];
        Ingredient ingredient = new Ingredient(ingredientSO);
        return ingredient;
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(IngredientSpawner))]
public class IngredientSpawnerEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        if (GUILayout.Button("Spawn Ingredient")) {
            var ingSpawner = (IngredientSpawner)target;
            ingSpawner.CurrentIngredient = ingSpawner.CreateIngredient();
            Debug.LogWarning("INGREDIENT CREATED");
            Debug.Log("Name: " + ingSpawner.CurrentIngredient.IngredientData.name);
        }
    }
}

#endif
