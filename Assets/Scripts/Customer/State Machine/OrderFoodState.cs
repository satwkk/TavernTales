using System;
using System.Collections;
using UnityEngine;

namespace LHC.Customer.StateMachine
{
    public class OrderFoodState : BaseState
    {
        private bool m_HasReachedOrderingZone;
        private Ingredient m_FoodToOrder;

        public OrderFoodState( Customer controller, CustomerData customerData ) : base( controller, customerData )
        {
            
        }

        public override void OnEnter()
        {
            m_FoodToOrder = IngredientSpawner.Instance.CreateIngredient();
            m_Customer.StartCoroutine(OrderFoodCoro());
        }

        public override void OnExit()
        {
            m_Customer.GetAnimationManager().PlayWalkingAnimation( false );
        }

        private IEnumerator OrderFoodCoro()
        {
            yield return new WaitForSeconds(1f);
            m_Customer.GetAnimationManager().PlayWalkingAnimation( true );
            m_Customer.StartCoroutine(FollowWayPoints(WayPointManager.instance.GetOrderFoodWayPoint(), () =>
            {
                m_HasReachedOrderingZone = true;
                m_Customer.GetAnimationManager().PlayWalkingAnimation( false );
                m_Customer.GetOrderManager().OrderFood( IngredientSpawner.Instance.CreateIngredient() );
            } ));
        }

        public override void OnTick()
        {
            if (m_HasReachedOrderingZone)
            {
                if (IsFoodAvailableInTime())
                {
                    Debug.Log( "Food is available now going to eating state" );
                }
            }
        }

        // CHECK IF THE FOOD IS AVAILABLE ON THE FOOD PLACING ZONE
        private bool IsFoodAvailableInTime()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                return true;
            }
            return false;
        }
    }
}
