using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LHC.Tavern
{
    using LHC.Customer;
    
    public class OrderingTable : MonoBehaviour
    {
        public static OrderingTable Instance;
        
        [field: SerializeField] public Customer CurrentOrderingCustomer { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
        }

        public bool IsOccupied()
        {
            return CurrentOrderingCustomer != null;
        }

        private void OnTriggerEnter(Collider other)
        {
            var customer = other.GetComponent<Customer>();
            if (customer is not null)
            {
                CurrentOrderingCustomer = customer;
            }
        }


        private void OnTriggerExit(Collider other)
        {
            var customer = other.GetComponent<Customer>();
            if (CurrentOrderingCustomer == customer)
            {
                CurrentOrderingCustomer = null;
            }
        }
    }
}