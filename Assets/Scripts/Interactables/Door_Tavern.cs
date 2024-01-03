using System;
using System.Collections;
using System.Collections.Generic;
using LHC.Customer;
using UnityEngine;

public class Door_Tavern : InteractableBase
{
    Quaternion innerRot = Quaternion.Euler( 0f, 90f, 0f );
    Quaternion outerRot = Quaternion.Euler( 0f, -90f, 0f );
    Quaternion targetRot = Quaternion.identity;
    private bool m_CanInteract = true;
    private bool m_IsOpen = false;
    public Action<Customer> OnCustomerEnter;

    public override void Interact( IInteractionActor interactingActor )
    {
        // IF THE DOOR IS ALREADY PLAYING AN ANIMATION THEN DON'T INTERACT WITH IT
        if (!m_CanInteract) return;

        if (interactingActor is Customer)
        {
            Debug.Log("A customer has entered");
            OnCustomerEnter?.Invoke(interactingActor as Customer);
        }

        m_IsOpen = !m_IsOpen;
        m_CanInteract = false;
        var dirToActor = ( interactingActor.GetInteractionPosition() - transform.position ).normalized;
        var dot = Vector3.Dot( dirToActor, transform.forward );
        StartCoroutine( StartInteract(dot) );
    }

    public bool bIsOpen()
    {
        return m_IsOpen == true;
    }

    IEnumerator StartInteract(float dot)
    {
        if ( dot > 0f ) targetRot = outerRot;
        else targetRot = innerRot;

        var rot = transform.rotation * targetRot;
        while ( transform.rotation != rot )
        {
            transform.rotation = Quaternion.Slerp( transform.rotation, rot, 2f * Time.deltaTime );
        }

        m_CanInteract = true;
        yield return null;
    }
}
