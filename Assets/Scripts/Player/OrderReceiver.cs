using System;
using System.Linq;
using UnityEngine;
using LHC.Customer;
using System.Collections;
using System.Collections.Generic;
using Timer = LHC.Globals.Timer;

[System.Serializable]
public struct CurrentOrderData 
{
    public Customer customer;
    public Ingredient ingredientToCook;
    public int foodsLeftToCompleteOrder;
    public Dictionary<string, bool> requiredFoodMap;
    public float timeLeftForOrder;

    public bool IsFoodAdded(Food food)
    {
        return requiredFoodMap[food.name] == true;
    }
}

public class OrderReceiver : MonoBehaviour
{
    public IFoodOrderService m_FoodServiceActor;
    private Timer m_FoodPreparationTimer;

    [Header("Current Order Data")][Space()]
    public CurrentOrderData m_CurrentOrderData;

    public static Action OnFoodServeSuccess_Event;
    public static Action OnFoodServeFail_Event;

    private void Awake()
    {
        m_FoodServiceActor = GetComponent<IFoodOrderService>();
    }

    private void Start()
    {
        m_CurrentOrderData.requiredFoodMap = new Dictionary<string, bool>();
        CustomerOrderManager.OnFoodOrdered_Event += OnFoodOrdered_Callback;
        CookingPot.OnFoodAddToPotEvent += OnFoodAddToPot_Callback;
    }

    private bool IsOrderComplete() 
    {
        return m_CurrentOrderData.foodsLeftToCompleteOrder == 0;
    }

    /// <summary>
    /// This callback is called when a food is added to the cooking pot by an IFoodOrderService object.
    /// </summary>
    /// <param name="food"> The last added food to the cooking pot </param>
    private void OnFoodAddToPot_Callback(Food food)
    {
        Food foundFood  = m_CurrentOrderData.ingredientToCook.IngredientData.requiredFoods.Where(x => x.name == food.name).First();
        Debug.LogError(foundFood == null);
        if (foundFood != null && !m_CurrentOrderData.IsFoodAdded(foundFood))
        {
            m_CurrentOrderData.requiredFoodMap[foundFood.name] = true;
            m_CurrentOrderData.foodsLeftToCompleteOrder--;
            Debug.Log("Correct food added to pot, one down");
        }
    }

    /// <summary>
    /// This callback is called when a customer reached the chef table and orders a food
    /// </summary>
    /// <param name="ingredient"> The ordered ingredient </param>
    private void OnFoodOrdered_Callback(Customer customer, Ingredient ingredient)
    {
        m_CurrentOrderData.customer = customer;
        m_CurrentOrderData.ingredientToCook = ingredient;
        m_CurrentOrderData.foodsLeftToCompleteOrder = ingredient.IngredientData.requiredFoods.Count;
        
        // Populate the required foods map in the current order data.
        ingredient.IngredientData.requiredFoods.ForEach(x => m_CurrentOrderData.requiredFoodMap[x.name] = false);

        // Start the cooking timer after the order is received.
        StartCoroutine(StartFoodPreparationTimer());
    }

    private IEnumerator StartFoodPreparationTimer()
    {
        var targetTimer = m_CurrentOrderData.ingredientToCook.IngredientData.prepareDuration;
        var currTimer = 0;
        while (currTimer <= targetTimer)
        {
            OnOrderServing();
            currTimer += 1;
            yield return new WaitForSeconds(1);
        }

        OnOrderServingComplete();
        yield return null;
    }

    private IEnumerator StartCheckIfOrderComplete()
    {
        yield return null;
    }

    /// <summary>
    /// Callback called when the currently serving food timer is active
    /// </summary>
    private void OnOrderServing() 
    {
        var isComplete = IsOrderComplete();
        Debug.LogWarning("Order state: " + isComplete);
        if (isComplete)
        {
            Debug.Log("Food is successfully ordered");
            StopAllCoroutines();
            //m_FoodPreparationTimer.StopTimer();
            OnFoodServeSuccess_Event?.Invoke();
        }
    }

    /// <summary>
    /// Callback called when the currently serving food timer is complete. This will only be called when the timer is over and the food is not served.
    /// Because if the food is served in between the serving time then all coroutines are stopped.
    /// </summary>
    private void OnOrderServingComplete()
    {
        Debug.LogError("Hello");
        OnFoodServeFail_Event?.Invoke();
    }

    private void OnDisable()
    {
        CustomerOrderManager.OnFoodOrdered_Event -= OnFoodOrdered_Callback;
        CookingPot.OnFoodAddToPotEvent -= OnFoodAddToPot_Callback;
    }
}
