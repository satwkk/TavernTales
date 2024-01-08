using System;
using System.Collections;
using UnityEngine;

public class InteractablePickup : InteractableBase
{

    private Vector3 _initialLocation;
    private IPickableActor _pickupActor;

    private void Awake() 
    {
    }

    public void Start()
    {
        _initialLocation = transform.position;
    }

    public void SetOwner(IPickableActor actor)
    {
        _pickupActor = actor;
    }

    public override void Interact(IInteractionActor interactingActor)
    {
        if (_pickupActor != null)
        {
            Debug.LogError("It is already picked up by someone");
            return;
        }

        if (interactingActor is not IPickableActor actor) 
        {
            Debug.LogError("Item is being picked up by someone who doesnt implement the IPickableActor Interface");
            return;
        }
        actor.PickupItem(this);
    }

    public void EnablePhysics()
    {
        transform.position = _initialLocation;
    }
}
