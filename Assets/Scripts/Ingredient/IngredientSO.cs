using UnityEngine;
using System.Collections.Generic;
using System;


[Serializable]
public struct IngredientData {
    public string name;
    public List<Food> requiredFoods;
    public float prepareDuration;
    public bool isServed;

    // CREATE A PREFAB WITH THE PREPARED INGREDIENT WHICH WILL BE SPAWNED WHEN THE INGREDIENT IS COOKED 
    // public GameObject CookedIngredientPrefab;

    public bool HasFoodInRequiredItems(Food food) {
        foreach (var f in requiredFoods) {
            if (f.name == food.name) {
                return true;
            }
        }
        return false;
    }
}