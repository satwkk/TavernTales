using UnityEngine;


public class InteractableBase : MonoBehaviour
{
    public string m_Id;

    private void Start() 
    {
        m_Id = this.name;
    }

    public virtual void Interact( IInteractionActor interactingActor ) { }

}
