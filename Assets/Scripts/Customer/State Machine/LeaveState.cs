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
            m_Customer.StartCoroutine(FollowWayPoints(WayPointManager.instance.leaveShopWayPoint, () => SwitchState(m_Customer.IdleState)));
        }

        public override void OnExit()
        {
        }

        public override void OnTick()
        {
        }
    }
}