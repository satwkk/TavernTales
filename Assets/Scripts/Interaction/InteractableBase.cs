using UnityEngine;


public class InteractableBase : MonoBehaviour
{
    public string Id;

    public virtual void Interact( IInteractionActor interactingActor ) { }

}
