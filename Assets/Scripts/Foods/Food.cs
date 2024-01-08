using System;
using UnityEngine;


public class Food : InteractableBase
{
    private MeshRenderer m_Renderer;
    private Collider m_Collider;

    public void Awake() 
    {
        m_Renderer = GetComponent<MeshRenderer>();
        m_Collider = GetComponent<Collider>();
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
