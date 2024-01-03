using UnityEngine;


public class InteractableBase : MonoBehaviour
{

    // TODO: Change the InteractionController to something abstract
    public virtual void Interact( IInteractionActor interactingActor ) { }

}
