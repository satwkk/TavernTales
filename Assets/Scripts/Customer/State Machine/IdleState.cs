using System;
using UnityEngine;
using LHC.Globals;
using System.Collections;
using System.Diagnostics;

namespace LHC.Customer.StateMachine
{
    public class IdleState : BaseState
    {
        private float m_MinIdleInterval = 1f;
        private float m_MaxIdleInterval = 2f;
        private float m_TargetIdleInterval;
        private float m_CurrentIdleTimer;

        public IdleState( Customer controller, CustomerData customerData ) : base( controller, customerData )
        {
        }

        public override void OnEnter()
        {
            UnityEngine.Debug.Log("Entering idle state");
            m_TargetIdleInterval = GetRandomWaitTime();
            m_CurrentIdleTimer = 0;
        }

        public override void OnExit()
        {
        }

        public override void OnTick()
        {
            if (m_CurrentIdleTimer >= m_TargetIdleInterval)
            {
                m_CurrentIdleTimer = 0;
                SwitchState(m_Customer.WanderState);
            }
            m_CurrentIdleTimer += Time.deltaTime;
        }

        private float GetRandomWaitTime()
        {
            return UnityEngine.Random.Range(m_MinIdleInterval, m_MaxIdleInterval);
        }
    }
}