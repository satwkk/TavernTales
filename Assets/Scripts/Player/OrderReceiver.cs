using LHC.Globals;
using NUnit.Framework.Constraints;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct CurrentOrderData 
{
    public Ingredient ingredientToCook;
    public int foodsLeftToCompleteOrder;
    public Dictionary<Food, bool> requiredFoodMap;

    public bool IsFoodAdded(Food food)
    {
        return requiredFoodMap[food] == true;
    }
}

public class OrderReceiver : MonoBehaviour
{
    public IFoodOrderService m_FoodServiceActor;

    [Header("Current Order Data")][Space()]
    public CurrentOrderData m_CurrentOrderData;
    public Food m_PickedFood;

    private void Awake()
    {
        m_FoodServiceActor = GetComponent<IFoodOrderService>();
    }

    private void Start()
    {
        m_CurrentOrderData.requiredFoodMap = new Dictionary<Food, bool>();
        
        CustomerOrderManager.OnFoodOrdered_Event += OnFoodOrdered_Callback;
        m_FoodServiceActor.OnPickFood_Event += OnPickFood_Callback;
        CookingPot.OnFoodAddToPot_Event += OnFoodAddToPot_Callback;
    }

    private bool IsOrderComplete() 
    {
        return m_CurrentOrderData.foodsLeftToCompleteOrder == 0;
    }

    /// <summary>
    /// Callback called when an IFoodOrderService actor picks up a food
    /// TODO: I dunno if this reference needs to be stayed here. It is already stored in InteractionController
    /// </summary>
    /// <param name="food"> The picked up food </param>
    private void OnPickFood_Callback(Food food)
    {
        m_PickedFood = food;
    }

    /// <summary>
    /// This callback is called when a food is added to the cooking pot by an IFoodOrderService object.
    /// </summary>
    /// <param name="food"> The last added food to the cooking pot </param>
    private void OnFoodAddToPot_Callback(Food food)
    {
        if (m_CurrentOrderData.ingredientToCook.IngredientData.requiredFoods.Contains(food) && m_CurrentOrderData.requiredFoodMap[food] == false)
        {
            m_CurrentOrderData.requiredFoodMap[food] = true;
            m_CurrentOrderData.foodsLeftToCompleteOrder--;
            Debug.Log( "Correct food added to pot, one down" );
        }
    }

    /// <summary>
    /// This callback is called when a customer reached the chef table and orders a food
    /// </summary>
    /// <param name="ingredient"> The ordered ingredient </param>
    private void OnFoodOrdered_Callback( Ingredient ingredient )
    {
        m_CurrentOrderData.ingredientToCook = ingredient;
        m_CurrentOrderData.foodsLeftToCompleteOrder = ingredient.IngredientData.requiredFoods.Count;
        
        // Populate the required foods map in the current order data.
        foreach (var food in ingredient.IngredientData.requiredFoods)
        {
            m_CurrentOrderData.requiredFoodMap[food] = false;
        }

        // Start the cooking timer after the order is received.
        Timer timer = new Timer(this, ingredient.IngredientData.prepareDuration, OnOrderServing, OnOrderServingComplete);
    }

    /// <summary>
    /// Callback called when the currently serving food timer is active
    /// </summary>
    private void OnOrderServing() 
    {
        Debug.LogWarning("Order state: " + IsOrderComplete());
    }

    /// <summary>
    /// Callback called when the currently serving food timer is complete.
    /// </summary>
    private void OnOrderServingComplete()
    {
    }

    private void OnDisable()
    {
        CustomerOrderManager.OnFoodOrdered_Event -= OnFoodOrdered_Callback;
        m_FoodServiceActor.OnPickFood_Event -= OnPickFood_Callback;
    }
}
