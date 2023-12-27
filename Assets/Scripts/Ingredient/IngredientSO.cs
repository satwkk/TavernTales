using UnityEngine;
using System.Collections.Generic;
using System;


[Serializable]
public struct IngredientData {
    public string name;
    public List<Food> requiredFoods;
    public float prepareDuration;
    public bool isPrepared;

    public bool HasFoodInRequiredItems(Food food) {
        foreach (var f in requiredFoods) {
            if (f.name == food.name) {
                return true;
            }
        }
        return false;
    }
}