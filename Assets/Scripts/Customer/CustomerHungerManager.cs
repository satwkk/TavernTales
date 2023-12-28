using LHC.Customer.StateMachine;
using System;
using System.Collections;
using UnityEngine;

namespace LHC.Customer
{
    [RequireComponent(typeof(Customer))]
    public class CustomerHungerManager : MonoBehaviour
    {
        private Customer m_Customer;
        private CustomerData m_CustomerData;
        private IState m_CurrentCustomerState;

        private void Awake()
        {
            m_Customer = GetComponent<Customer>();
            m_CustomerData = m_Customer.GetCustomerData();
            m_CurrentCustomerState = m_CustomerData.currentState;
        }

        private IEnumerator StartHungerCalculations()
        {
            yield return null;
        }

        private void CalculateHunger()
        {
        }
    }
}