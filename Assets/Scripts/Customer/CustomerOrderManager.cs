using LHC.Customer;
using System;
using UnityEngine;

public class CustomerOrderManager : MonoBehaviour
{
    private Customer m_Customer;
    public Ingredient CurrentOrderedFood { get; set; }
    public static Action<Customer, Ingredient> OnFoodOrdered_Event;
    public Action OnOrderComplete_Event;
    public Action OnOrderFail_Event;

    private void Start()
    {
        m_Customer = GetComponent<Customer>();
    }

    private void OnFoodServeFail_Callback()
    {
        OnOrderFail_Event?.Invoke();
    }

    private void OnFoodServeSuccess_Callback()
    {
        OnOrderComplete_Event?.Invoke();
    }

    // TODO: Maybe pass by reference
    public void OrderFood(Ingredient ingredient)
    {
        CurrentOrderedFood = ingredient;
        OnFoodOrdered_Event?.Invoke( m_Customer, CurrentOrderedFood );
    }

    private void OnDisable() 
    {
    }
}
