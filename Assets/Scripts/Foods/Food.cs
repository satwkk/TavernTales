using System;
using UnityEngine;

// public class Food : InteractablePickup {
public class Food : InteractableBase 
{
    private MeshRenderer m_Renderer;
    private Collider m_Collider;
    public static Action<Food> OnFoodPickup;

    public void Awake() 
    {
        m_Renderer = GetComponent<MeshRenderer>();
        m_Collider = GetComponent<Collider>();
    }

    private void Start()
    {
    }

    public override void Interact( IInteractionActor interactingActor ) 
    {
        if (interactingActor is not IFoodOrderService foodService)
        {
            Debug.LogError("Not implemented IInteractionActor");
            return;
        }
        foodService.PickupFood(this);
    }
}
