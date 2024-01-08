using System.Collections;
using System.Collections.Generic;
using LHC.Customer;
using LHC.Globals;
using LHC.Tavern;
using UnityEngine;

public class OnSittingAnimation : StateMachineBehaviour
{
    [field: SerializeField] public Transform HipRaycastTransform { get; set; }
    public Transform OwnerTransform { get; set; }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OwnerTransform = animator.transform;
        HipRaycastTransform = OwnerTransform.Find(Constants.HIP_RAYCAST_TRANSFORM);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Vector3 targetPos = Vector3.zero;
        // if (Physics.Raycast(HipRaycastTransform.position, Vector3.down, out RaycastHit hitInfo, 100f))
        // {
        //     Seat seat = hitInfo.collider.GetComponent<Seat>();
        //     if (seat != null) 
        //     {
        //         Debug.Log("we found a seat nigga");
        //         Debug.DrawLine(HipRaycastTransform.position, HipRaycastTransform.position + (Vector3.down * 100f), Color.blue);
        //         targetPos = hitInfo.point;
        //         targetPos.y -= 2f;
        //         OwnerTransform.position = Vector3.Lerp(OwnerTransform.position, targetPos, 1f * Time.deltaTime);
        //     }
        // }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
