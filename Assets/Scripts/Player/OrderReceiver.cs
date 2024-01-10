using System;
using System.Linq;
using UnityEngine;
using LHC.Customer;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CurrentOrderData 
{
    public Customer customer;
    public Ingredient ingredientToCook;
    public int foodsLeftToCompleteOrder;
    public Dictionary<string, bool> requiredFoodMap;
    public float CurrentCookingTimer;

    public bool IsFoodAdded(Food food)
    {
        return requiredFoodMap[food.Id] == true;
    }
}

public class OrderReceiver : MonoBehaviour
{

    [Header("Current Order Data")][Space()]
    public CurrentOrderData CurrentOrderData;
    
    public IFoodOrderService FoodServiceActor { get; private set; }

    [field: SerializeField] public bool IsOrderReceived { get; private set; } = false;

    private void Awake()
    {
        FoodServiceActor = GetComponent<IFoodOrderService>();
    }

    private void Start()
    {
        CustomerOrderManager.OnFoodOrdered_Event += OnFoodOrdered_Callback;
        CookingPot.OnFoodAddToPotEvent += OnFoodAddToPot_Callback;
    }

    private void Update()
    {
        if (!IsOrderReceived) {
            Debug.LogError("Is is not recieved");
            return;
        }

        if (!HasAllRequiredFoodsAdded()) {
            Debug.LogError("All required foods are not added");
            return;
        }

        if (!IsOrderReadyToServe())
        {
            CurrentOrderData.CurrentCookingTimer += Time.deltaTime;
            return;
        }
        Debug.LogError("cooking timer complete, please take the food");
        CookingPot.Instance.CreateCookedIngredient(CurrentOrderData.ingredientToCook);
        Cleanup();
    }

    private void Cleanup()
    {
        IsOrderReceived = false;
        CurrentOrderData = null;
    }

    private bool IsOrderReadyToServe() 
    {
        return CurrentOrderData.CurrentCookingTimer >= CurrentOrderData.ingredientToCook.IngredientData.PrepationDuration &&  
            !CurrentOrderData.ingredientToCook.IngredientData.IsServed;
    }

    private bool HasAllRequiredFoodsAdded() 
    {
        return CurrentOrderData.foodsLeftToCompleteOrder == 0;
    }

    private void OnFoodAddToPot_Callback(Food food)
    {
        Food foundFood  = CurrentOrderData.ingredientToCook.IngredientData.RequiredFoods.Where(x => x.Id == food.Id).First();
        if (foundFood != null && !CurrentOrderData.IsFoodAdded(foundFood))
        {
            CurrentOrderData.requiredFoodMap[foundFood.Id] = true;
            CurrentOrderData.foodsLeftToCompleteOrder--;
        }
    }

    private void OnFoodOrdered_Callback(Customer customer, Ingredient ingredient)
    {
        this.IsOrderReceived = true;
        CurrentOrderData = new CurrentOrderData() 
        {
            customer = customer,
            ingredientToCook = ingredient,
            foodsLeftToCompleteOrder = ingredient.IngredientData.RequiredFoods.Count,
            requiredFoodMap = new Dictionary<string, bool>(),
            CurrentCookingTimer = 0
        };

        // Populate the required foods map in the current order data
        CurrentOrderData.ingredientToCook.IngredientData.RequiredFoods.ForEach(x => CurrentOrderData.requiredFoodMap.Add(x.Id, false));
    }

    private void OnDisable()
    {
        CustomerOrderManager.OnFoodOrdered_Event -= OnFoodOrdered_Callback;
        CookingPot.OnFoodAddToPotEvent -= OnFoodAddToPot_Callback;
    }
}
