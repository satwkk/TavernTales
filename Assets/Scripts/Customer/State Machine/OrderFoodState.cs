using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace LHC.Customer.StateMachine
{
    public class OrderFoodState : BaseState
    {
        public bool IsFoodOrdered { get; set; }
        public Ingredient ingredientToOrder;
        public Ingredient IngredientToOrder { get => ingredientToOrder; set => ingredientToOrder = value; }
        public float CurrentWaitTimer { get; private set; } = 0f;

        public OrderFoodState(Customer controller, CustomerData customerData) : base( controller, customerData )
        {
        }

        public override void OnEnter()
        {
            m_Customer.StartCoroutine(OrderFoodCoroutine());
        }

        public override void OnExit()
        {
            IsFoodOrdered = false;
            IngredientToOrder = null;
            CurrentWaitTimer = 0f;
            m_Customer.AnimationManager.PlayWalkingAnimation(false);
        }

        private IEnumerator OrderFoodCoroutine()
        {
            yield return new WaitForSeconds(1f);
            m_Customer.AnimationManager.PlayWalkingAnimation(true);
            m_Customer.StartCoroutine(FollowWayPoints(WayPointManager.instance.GetOrderFoodWayPoint(), after: () =>
            {
                IngredientToOrder = IngredientSpawner.Instance.CreateIngredient();
                IngredientToOrder.IngredientData.OnServe += OnServe;

                m_Customer.AnimationManager.PlayWalkingAnimation(false);
                m_Customer.OrderManager.OrderFood(ingredientToOrder);

                IsFoodOrdered = true;
            }));
        }

        private void OnServe()
        {
            Debug.Log("Food has been served");
            IngredientToOrder.IngredientData.OnServe -= OnServe;
            SwitchState(m_Customer.EatState);
        }

        public override void OnTick()
        {
            if (IsFoodOrdered == false)
                return;

            if (CurrentWaitTimer >= IngredientToOrder.IngredientData.CustomerWaitingTimer && IngredientToOrder.IngredientData.IsServed.Equals(false))
            {
                Debug.LogError("Customer has waited long enough and no food has been served");
                IngredientToOrder.IngredientData.OnServe -= OnServe;
                // SWITCH TO LEAVING STATE
                SwitchState(m_Customer.IdleState);
            }

            CurrentWaitTimer += Time.deltaTime;
        }
    }
}
