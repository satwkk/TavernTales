using System.Collections;
using System.Collections.Generic;
using LHC.Customer;
using UnityEngine;

public class QueueWayPoint : WayPoint {
    public Customer customer;

    private void OnTriggerEnter(Collider other) {
    }
}

public class CustomerQueueManager : MonoBehaviour
{
    public Dictionary<WayPoint, Customer> Customer;
}
