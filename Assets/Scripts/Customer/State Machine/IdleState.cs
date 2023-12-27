using System;
using UnityEngine;
using LHC.Globals;
using System.Collections;

namespace LHC.Customer.StateMachine
{
    public class IdleState : BaseState
    {
        public IdleState( Customer controller, CustomerData customerData ) : base( controller, customerData )
        {
        }

        public override void OnEnter()
        {
            Debug.Log( "Entering idle state" );
            Timer t = new Timer( m_Customer, 5f, null, () => SwitchState( m_Customer.m_WanderState ) );
        }

        public override void OnExit()
        {
        }

        public override void OnTick()
        {
        }
    }
}