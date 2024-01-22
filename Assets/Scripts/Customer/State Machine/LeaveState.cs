using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace LHC.Customer.StateMachine
{
    public class LeaveState : BaseState
    {
        public LeaveState(Customer controller, CustomerData customerData) : base(controller, customerData)
        {
        }

        public override void OnEnter()
        {
            Debug.Log("Entering leave state");

            // START THE WALKING ANIMATION
            customer.AnimationManager.PlayWalkingAnimation(true);
            customer.StartCoroutine(NavMeshMoveToCoro(WayPointManager.instance.approachShopWayPoint[0].transform.position,
            customerData.locomotionData.walkSpeed, 3, OnLeaveShop));
        }

        private void OnLeaveShop()
        {
            Debug.Log("Leaving shop successfull");
            SwitchState(customer.IdleState);
        }

        public override void OnExit()
        { }

        public override void OnTick()
        { }
    }
}