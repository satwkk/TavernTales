using System;
using UnityEngine;


public class Food : InteractableBase
{
    protected virtual void Awake() 
    {
    }

    private void Start ()
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

    // public void OnSpawn() 
    // {
    //     rigidbody.isKinematic = false;
    //     rigidbody.AddForce((Vector3.up + UnityEngine.Random.insideUnitSphere).normalized * 2f);
    // }
}
