using System;
using System.Collections;
using UnityEngine;

namespace LHC.Customer.StateMachine
{
    public class OrderFoodState : BaseState
    {
        public OrderFoodState(Customer controller, CustomerData customerData) : base( controller, customerData )
        {
        }

        private void OnOrderComplete_Callback()
        {
            SwitchState(m_Customer.EatState);
        }

        private void OnOrderFail_Callback()
        {
            Debug.Log("order failed customer is now angy.");
        }

        public override void OnEnter()
        {
            m_Customer.StartCoroutine(OrderFoodCoroutine());
        }

        public override void OnExit()
        {
            m_Customer.GetAnimationManager().PlayWalkingAnimation( false );
            m_Customer.GetOrderManager().OnOrderComplete_Event -= OnOrderComplete_Callback;
            m_Customer.GetOrderManager().OnOrderFail_Event -= OnOrderFail_Callback;
        }

        private IEnumerator OrderFoodCoroutine()
        {
            yield return new WaitForSeconds(1f);
            m_Customer.GetAnimationManager().PlayWalkingAnimation( true );
            m_Customer.StartCoroutine(FollowWayPoints(WayPointManager.instance.GetOrderFoodWayPoint(), after: () =>
            {
                m_Customer.GetAnimationManager().PlayWalkingAnimation( false );
                m_Customer.GetOrderManager().OrderFood( IngredientSpawner.Instance.CreateIngredient() );
                m_Customer.GetOrderManager().OnOrderComplete_Event += OnOrderComplete_Callback;
                m_Customer.GetOrderManager().OnOrderFail_Event += OnOrderFail_Callback;
            } ));
        }

        public override void OnTick()
        {
        }
    }
}
