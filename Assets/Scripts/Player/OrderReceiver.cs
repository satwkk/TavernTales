using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The purpose of this script is to check if the ingredient is prepared inside the time interval or not
/// </summary>
public class OrderReceiver : MonoBehaviour
{
    public IFoodOrderService m_FoodServiceActor;
    public Ingredient m_FoodToServe;
    public Dictionary<Food, bool> m_FoodsToCompleteOrder;
    public Food m_PickedFood;

    private void Awake()
    {
        m_FoodServiceActor = GetComponent<IFoodOrderService>();
    }

    private void Start()
    {
        CustomerOrderManager.OnFoodOrdered_Event += OnFoodOrdered_Callback;
        m_FoodServiceActor.OnPickFood_Event += OnPickFood_Callback;
        CookingPot.OnFoodAddToPot_Event += OnFoodAddToPot_Callback;
    }


    private void Update()
    {
        if ( !HasFoodToServe() ) return;
    }

    private bool IsOrderComplete()
    {
        return false;
    }

    private void OnPickFood_Callback(Food food)
    {
        m_PickedFood = food;
    }

    private void OnFoodAddToPot_Callback(Food food)
    {
        if (m_PickedFood == food && m_FoodsToCompleteOrder.ContainsKey(food))
        {
            Debug.Log( "Correct food added to pot, one down" );
            m_FoodsToCompleteOrder[m_PickedFood] = true;
        }
    }

    public void OnFoodOrdered_Callback( Ingredient ingredient )
    {
        m_FoodToServe = ingredient;
        foreach (Food food in ingredient.IngredientData.requiredFoods)
        {
            m_FoodsToCompleteOrder[food] = false;
        }
    }

    private bool HasFoodToServe() { return m_FoodToServe != null; }

    private void OnDisable()
    {
        CustomerOrderManager.OnFoodOrdered_Event -= OnFoodOrdered_Callback;
        m_FoodServiceActor.OnPickFood_Event -= OnPickFood_Callback;
    }
}
