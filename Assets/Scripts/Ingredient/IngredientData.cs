using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

// [Serializable]
[CreateAssetMenu(fileName = "IngredientData", menuName = "Ingredient/Ingredient Data")]
public class IngredientData : ScriptableObject
{
    public string Name;
    public float PreparationDuration;
    public float EatingDuration;
    public float CustomerWaitingTimer => PreparationDuration * 3;
    public Image FoodIcon;
    public List<Food> RequiredFoods;
    public GameObject CookedIngredientPrefab;

    // public IngredientData(IngredientData other) 
    // {
    //     this.Name = other.Name;
    //     this.RequiredFoods = other.RequiredFoods;
    //     this.PreparationDuration = other.PreparationDuration;
    //     this.CookedIngredientPrefab = other.CookedIngredientPrefab;
    //     this.EatingDuration = other.EatingDuration;
    // }
}