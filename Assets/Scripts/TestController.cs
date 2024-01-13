using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour
{
    public CharacterController characterController;
    public float speed = 10f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.y = transform.position.y;
            transform.Translate(mouseWorldPos - transform.position, Space.Self);
        }
    }
}
