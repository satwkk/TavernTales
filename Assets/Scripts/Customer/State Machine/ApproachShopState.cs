using System.Collections;
using UnityEngine;

namespace LHC.Customer.StateMachine
{
    public class ApproachShopState : BaseState
    {
        private Ingredient m_Order;

        public ApproachShopState( Customer controller, CustomerData customerData ) : base( controller, customerData )
        {
        }

        public override void OnEnter()
        {
            m_Order = IngredientSpawner.Instance.CreateIngredient();
        }

        public override void OnExit()
        {
        }

        public override void OnTick()
        {
        }
    }
}