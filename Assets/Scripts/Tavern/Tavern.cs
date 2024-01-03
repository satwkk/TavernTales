using System.Collections.Generic;
using LHC.Customer;
using UnityEngine;

public class Tavern : MonoBehaviour
{
    public Door_Tavern m_TavernDoor;
    public Transform m_OrderingLocation;
    public List<Customer> m_Customers;

    public static Tavern instance;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void OnCustomerEnter_Callback(Customer customer)
    {
        m_Customers.Add(customer);
    }

    private void OnEnable()
    {
        m_TavernDoor.OnCustomerEnter += OnCustomerEnter_Callback;
    }

    private void OnDisable()
    {
        m_TavernDoor.OnCustomerEnter -= OnCustomerEnter_Callback;
    }   
}
