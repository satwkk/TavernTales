using System.Collections;
using System.Collections.Generic;
using LHC.Customer;
using UnityEngine;


public class CustomerSpawnerDebug : MonoBehaviour
{
    public Customer[] customers;

    void Update() 
    {
        if (Input.GetKeyDown(KeyCode.H)) 
        {
            customers[0].SwitchStateDebug();
        }
        if (Input.GetKeyDown(KeyCode.J)) 
        {
            customers[1].SwitchStateDebug();
        }
        if (Input.GetKeyDown(KeyCode.K)) 
        {
            customers[2].SwitchStateDebug();
        }
        if (Input.GetKeyDown(KeyCode.L)) 
        {
            customers[3].SwitchStateDebug();
        }
    }
}
