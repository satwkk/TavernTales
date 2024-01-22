using LHC.Globals;
using UnityEngine;
using UnityEditor;

namespace LHC.Tavern
{
    using LHC.Customer;
    
    public class OrderingTable : MonoBehaviour
    {
        public static OrderingTable Instance;
        public Transform orderingLocationTransform;
        public Customer currentOrderingCustomer;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }

            orderingLocationTransform = transform.Find(Constants.ORDERING_ZONE_SOCKET).transform;
        }

        private void Start() {
        }

        public bool IsOccupied()
        {
            return currentOrderingCustomer != null;
        }

        private void OnTriggerEnter(Collider other)
        {
            var customer = other.GetComponent<Customer>();
            if (!IsOccupied())
            {
                currentOrderingCustomer = customer;
            }
        }


        private void OnTriggerExit(Collider other)
        {
            var customer = other.GetComponent<Customer>();
            if (currentOrderingCustomer != null && currentOrderingCustomer == customer)
            {
                currentOrderingCustomer = null;
            }
        }

        private void OnDrawGizmos() {
            Handles.DrawWireCube(orderingLocationTransform.position, Vector3.one * .2f);
        }
    }
}