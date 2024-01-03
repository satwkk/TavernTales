using UnityEngine;

public class InteractablePickup : InteractableBase
{

    Vector3 initialLocation;

    protected IPickableActor m_PickupActor;

    public void Start()
    {
        initialLocation = transform.position;
    }

    public override void Interact( IInteractionActor pickupActor )
    {
        m_PickupActor = pickupActor as IPickableActor;
        m_PickupActor?.PickupItem( this );
    }

    public virtual void Use() 
    {
    }

    public void Drop() 
    {
        if (m_PickupActor == null) 
        {
            Debug.LogError("This item is not picked up by anyone, this is prolly a BUG.");
            return;
        }
        m_PickupActor.DropItem(this);
        m_PickupActor = null;
    }
}
