using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    public float hMoveInputValue { get; private set; }
    public float vMoveInputValue { get; private set; }

    public float xMouseInputValue { get; private set; }
    public float yMouseInputValue { get; private set; }

    public bool isInteractPressed { get; private set; }


    // EVENTS
    public Action OnDropButtonPressed { get; set; }

    void Update() {
        hMoveInputValue = Input.GetAxisRaw("Horizontal");
        vMoveInputValue = Input.GetAxisRaw("Vertical");

        xMouseInputValue = Input.GetAxis("Mouse X");
        yMouseInputValue = Input.GetAxis("Mouse Y");

        isInteractPressed = Input.GetKeyDown(KeyCode.E);

        if (Input.GetKeyDown(KeyCode.G)) 
            OnDropButtonPressed?.Invoke();
    }
}
