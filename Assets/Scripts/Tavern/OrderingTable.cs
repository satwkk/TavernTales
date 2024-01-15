using System.Collections.Generic;
using UnityEngine;

namespace LHC.Tavern
{
    using System.Collections;
    using LHC.Customer;
    using UnityEditor;

    public class OrderingTable : MonoBehaviour
    {
        public static OrderingTable Instance;
        private Vector3 m_OrderOffsetPosition = new Vector3(6.69999981f, 1.60000002f, -13.3999996f);
        public Vector3 OrderingPosition => m_OrderOffsetPosition;

        [field: SerializeField] public Customer CurrentOrderingCustomer { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
        }

        private void Start() {
        }

        public bool IsOccupied()
        {
            return CurrentOrderingCustomer != null;
        }

        private void OnTriggerEnter(Collider other)
        {
            var customer = other.GetComponent<Customer>();
            if (!IsOccupied())
            {
                CurrentOrderingCustomer = customer;
            }
        }


        private void OnTriggerExit(Collider other)
        {
            var customer = other.GetComponent<Customer>();
            if (CurrentOrderingCustomer != null && CurrentOrderingCustomer == customer)
            {
                CurrentOrderingCustomer = null;
            }
        }

        private void OnDrawGizmos() {
            Handles.DrawWireCube(m_OrderOffsetPosition, Vector3.one * .2f);
        }
    }
}