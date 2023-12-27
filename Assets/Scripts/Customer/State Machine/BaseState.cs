using System;
using UnityEngine;

namespace LHC.Customer.StateMachine
{
    public abstract class BaseState : IState
    {
        protected Customer m_Customer;
        protected CustomerData m_CustomerData;

        public BaseState(Customer controller, CustomerData customerData)
        {
            m_Customer = controller;
            m_CustomerData = customerData;
        }

        public abstract void OnEnter();

        public abstract void OnExit();

        public abstract void OnTick();

        public void SwitchState(IState newState)
        {
            m_Customer.m_CustomerData.currentState.OnExit();
            m_Customer.m_CustomerData.currentState = newState;
            m_Customer.m_CustomerData.currentState.OnEnter();
        }
    }
}