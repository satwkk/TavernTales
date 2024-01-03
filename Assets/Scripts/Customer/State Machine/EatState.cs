using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LHC.Customer.StateMachine
{
    public class EatState : BaseState
    {
        public EatState( Customer controller, CustomerData customerData ) : base( controller, customerData )
        {
        }

        public override void OnEnter()
        {
            throw new System.NotImplementedException();
        }

        public override void OnExit()
        {
            throw new System.NotImplementedException();
        }

        public override void OnTick()
        {
            throw new System.NotImplementedException();
        }
    }
}

