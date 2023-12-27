using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickableActor {
    public Transform PickupHolder { get; set; }

    public bool HasPickupInHands();
    public void PickupItem(InteractablePickup pickup);
}
