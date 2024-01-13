using LHC.Customer;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public Customer[] customers;

    public void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }
}
