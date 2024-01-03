using System.Collections.Generic;
using UnityEngine;

namespace LHC.Customer.StateMachine
{
    public class ApproachShopState : BaseState
    {
        private float m_InteractableCheckRange = 4f;

        public ApproachShopState( Customer controller, CustomerData customerData) : base( controller, customerData )
        {
        }

        public override void OnEnter()
        {
            DebugSpawnCustomer();
            m_Customer.GetAnimationManager().PlayWalkingAnimation(true);
            m_Customer.StartCoroutine( FollowWayPoints( WayPointManager.instance.approachShopWayPoint, () =>
            {
                SwitchState( m_Customer.m_OrderState );
            } ) );
        }

        public override void OnExit()
        {
            m_Customer.GetAnimationManager().PlayWalkingAnimation(false);
        }

        public override void OnTick()
        {
            // CHECK FOR DOOR IN THE WAY
            CheckForShopDoor();
        }

        private void DebugSpawnCustomer()
        {
            m_Customer.m_CharacterController.enabled = false;
            m_Customer.transform.position = m_Customer.m_DebugSpawnLocation.position;
            m_Customer.m_CharacterController.enabled = true;
        }

        private void CheckForShopDoor()
        {
            if (Physics.Raycast(m_Customer.transform.position, m_Customer.transform.forward, out RaycastHit hitInfo, m_InteractableCheckRange))
            {
                var door = hitInfo.collider.GetComponent<Door_Tavern>();
                if (door != null && !door.bIsOpen())
                {
                    door.Interact(m_Customer);
                }
            }
        }
    }
}