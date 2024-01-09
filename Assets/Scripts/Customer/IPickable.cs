using UnityEngine;

public interface IPickable
{
    public IPickableActor Owner { get; set; }
    void PickUp(Transform pickupHolder);
    void Drop(Transform pickupHolder);
}