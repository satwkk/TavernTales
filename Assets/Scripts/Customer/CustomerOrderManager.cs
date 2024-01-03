using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerOrderManager : MonoBehaviour
{
    private Ingredient m_OrderedFood;
    public static Action<Ingredient> OnFoodOrdered_Event;

    public void OrderFood(Ingredient ingredient)
    {
        m_OrderedFood = ingredient;
        OnFoodOrdered_Event?.Invoke( m_OrderedFood );
        StartCoroutine( StartServingWaitTimer() );
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

    void Start()
    {
    }

    void Update()
    {
    }
}
