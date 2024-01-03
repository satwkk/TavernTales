using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Customer Data", menuName = "Customer/Customer Settings")]
public class CustomerSO : ScriptableObject
{
    [Header("Interactable Detection Range")] [Space()]
    public float findInteractableRange;

    [Header("Wandering Settings")][Space()]
    public float wanderWalkSpeed;
    public float wanderRadius;

    [Header("Food Order Settings")][Space()]
    public float foodOrderWalkSpeed;
}
