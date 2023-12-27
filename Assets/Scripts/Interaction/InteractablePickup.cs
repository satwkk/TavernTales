using UnityEngine;

public class InteractablePickup : InteractableBase {

    Vector3 initialLocation;

    // CHANGE THIS TO SOMETHING MORE ABSTRACT
    protected InteractionController m_Owner;

    public void Start() {
        initialLocation = transform.position;
    }

    public override void Interact(InteractionController interactingActor) {
        m_Owner = interactingActor;
        m_Owner.PickupItem(this);
    }

    public void SetOwner(InteractionController ownerController) {
        if (m_Owner != null) {
            Debug.LogError("Owner is already assigned");
            return;
        }
        m_Owner = ownerController;
    }

    public virtual void Use() {
    }

    public void Drop() {
        if (m_Owner == null) {
            Debug.LogError("This item is not picked up by anyone, this is prolly a BUG.");
            return;
        }
        m_Owner.DropItem();
        m_Owner = null;
    }
}
