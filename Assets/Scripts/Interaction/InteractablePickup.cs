using System.Collections;
using UnityEngine;

public class InteractablePickup : InteractableBase
{

    Vector3 initialLocation;
    protected IPickableActor m_PickupActor;

    private void Awake() 
    {
    }

    public void Start()
    {
        initialLocation = transform.position;
    }

    public void SetOwner(IPickableActor actor)
    {
        m_PickupActor = actor;
    }

    public override void Interact(IInteractionActor interactingActor)
    {
        if (m_PickupActor != null)
        {
            Debug.Log("It is already picked up by someone");
            return;
        }

        var actor = interactingActor as IPickableActor;
        if (actor == null) 
        {
            Debug.LogError("Item is being picked up by someone who doesnt implement the IPickableActor Interface");
            return;
        }
        actor.PickupItem(this);
    }

    public void EnablePhysics()
    {
        transform.position = initialLocation;
    }
}
