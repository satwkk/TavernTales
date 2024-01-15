using System.Collections;
using UnityEngine;

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
            m_Customer.AnimationManager.PlayWalkingAnimation(true);

            m_Customer.StartCoroutine(NavMeshMoveTo(WayPointManager.instance.approachShopWayPoint[0].transform.position,
            m_CustomerData.locomotionData.walkSpeed, 3, OnLeaveShop));
        }

        private void OnLeaveShop()
        {
            Debug.Log("Leaving shop successfull");
            SwitchState(m_Customer.IdleState);
        }

        public override void OnExit()
        { }

        public override void OnTick()
        { }
    }
}