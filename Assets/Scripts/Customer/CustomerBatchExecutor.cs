using LHC.Customer;
using UnityEngine;

public class CustomerBatchExecutor : MonoBehaviour
{
    public Customer[] customers;

    private void Awake() {
        customers = FindObjectsOfType<Customer>();
    }

    private void Update() 
    {
        for(int i = 0; i < customers.Length; i++)
        {
        }
    }
}


