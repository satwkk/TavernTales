using LHC.Customer;
using LHC.Globals;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerOrderManager : MonoBehaviour
{
    private Customer m_Customer;
    private Ingredient m_OrderedFood;
    public static Action<Customer, Ingredient> OnFoodOrdered_Event;
    public Action OnOrderComplete_Event;
    public Action OnOrderFail_Event;

    private void Start()
    {
        m_Customer = GetComponent<Customer>();
        OrderReceiver.OnFoodServeSuccess_Event += OnFoodServeSuccess_Callback;
        OrderReceiver.OnFoodServeFail_Event += OnFoodServeFail_Callback;
    }

    private void OnFoodServeFail_Callback()
    {
        OnOrderFail_Event?.Invoke();
    }

    private void OnFoodServeSuccess_Callback()
    {
        OnOrderComplete_Event?.Invoke();
    }

    public void OrderFood(Ingredient ingredient)
    {
        m_OrderedFood = ingredient;
        OnFoodOrdered_Event?.Invoke( m_Customer, m_OrderedFood );
    }

    private void OnDisable() 
    {
        OrderReceiver.OnFoodServeSuccess_Event -= OnFoodServeSuccess_Callback;
        OrderReceiver.OnFoodServeFail_Event -= OnFoodServeFail_Callback;
    }

    private IEnumerator StartServingWaitTimer()
    {
        var targetTimer = m_OrderedFood.IngredientData.prepareDuration;
        var currTimer = 0;
        while (currTimer < targetTimer)
        {
            currTimer += 1;
            yield return new WaitForSeconds( 1f );
        }

        if (!m_OrderedFood.IngredientData.isServed)
        {
            Debug.Log( "Food was not served, customer is angry" );
        }
    }
}
