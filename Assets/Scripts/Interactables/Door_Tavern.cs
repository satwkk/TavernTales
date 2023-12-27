using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Tavern : InteractableBase
{
    Quaternion innerRot = Quaternion.Euler( 0f, 90f, 0f );
    Quaternion outerRot = Quaternion.Euler( 0f, -90f, 0f );
    Quaternion targetRot = Quaternion.identity;

    public override void Interact( InteractionController interactingActor )
    {
        var dirToActor = ( interactingActor.transform.position - transform.position ).normalized;
        var dot = Vector3.Dot( dirToActor, transform.forward );
        StartCoroutine( StartInteract(dot) );
    }

    IEnumerator StartInteract(float dot)
    {
        if ( dot > 0f )
        {
            targetRot = outerRot;
        }
        else
        {
            targetRot = innerRot;
        }

        var rot = transform.rotation * targetRot;
        while ( transform.rotation != rot )
        {
            transform.rotation = Quaternion.Slerp( transform.rotation, rot, 2f * Time.deltaTime );
            yield return null;
        }
    }
}
