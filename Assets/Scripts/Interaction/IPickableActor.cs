using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickableActor : IInteractionActor {
    public Transform PickupHolder { get; set; }

    public bool HasPickupInHands();
    public void PickupItem(InteractableBase pickup);
    public void DropItem(InteractableBase pickup);
}
