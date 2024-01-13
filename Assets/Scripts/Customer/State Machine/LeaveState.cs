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
            // START THE WALKING ANIMATION
            m_Customer.AnimationManager.PlayWalkingAnimation(true);

            // FOLLOW THE WAYPOINTS TO LEAVE THE SHOP
            m_Customer.StartCoroutine(FollowWayPoints(WayPointManager.instance.leaveShopWayPoint, OnLeaveShop));
        }

        public override void OnExit()
        {
        }

        public override void OnTick()
        {
        }

        private void OnLeaveShop()
        {
            Debug.Log("Left the shop going back to idle");
            SwitchState(m_Customer.IdleState);
        }
    }
}