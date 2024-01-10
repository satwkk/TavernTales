using UnityEngine;
using System.Collections.Generic;
using System;


[Serializable]
public class IngredientData {
    public string Name;
    public List<Food> RequiredFoods;
    public float PrepationDuration;
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

    // TODO: Maybe move this to customer data later because different kind of customers with different mood and race might have different waiting time.
    public float CustomerWaitingTimer => PrepationDuration * 3;

    // CREATE A PREFAB WITH THE PREPARED INGREDIENT WHICH WILL BE SPAWNED WHEN THE INGREDIENT IS COOKED 
    public GameObject CookedIngredientPrefab;

    public IngredientData(IngredientData other) 
    {
        this.Name = other.Name;
        this.RequiredFoods = other.RequiredFoods;
        this.PrepationDuration = other.PrepationDuration;
        this.OnServe = other.OnServe;
        this.isServed = other.isServed;
        this.CookedIngredientPrefab = other.CookedIngredientPrefab;
    }

    public bool HasFoodInRequiredItems(Food food) {
        foreach (var f in RequiredFoods) {
            if (f.name == food.name) {
                return true;
            }
        }
        return false;
    }
}