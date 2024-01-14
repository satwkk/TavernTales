using System.Collections.Generic;
using UnityEngine;

namespace LHC.Customer.StateMachine
{
    public class ApproachShopState : BaseState
    {

        public ApproachShopState( Customer controller, CustomerData customerData) : base( controller, customerData )
        {
        }

        public override void OnEnter()
        {
            m_Customer.AnimationManager.PlayWalkingAnimation(true);
            m_Customer.StartCoroutine( FollowWayPoints( WayPointManager.instance.approachShopWayPoint, () =>
            {
                SwitchState( m_Customer.OrderState );
            } ) );
        }

        public override void OnExit()
        {
            m_Customer.AnimationManager.PlayWalkingAnimation(false);
        }

        public override void OnTick()
        {
            // CHECK FOR DOOR IN THE WAY
            CheckForShopDoor();
        }

        private void CheckForShopDoor()
        {
            // if (Physics.Raycast(m_Customer.transform.position, m_Customer.transform.forward, out RaycastHit hitInfo, m_CustomerData.interactionData.interactionRange))
            if (Physics.Raycast(
                m_CustomerData.interactionData.interactionRaycastPosition.position, 
                m_CustomerData.interactionData.interactionRaycastPosition.forward, 
                out RaycastHit hitInfo, 
                m_CustomerData.interactionData.interactionRange
                )
            )
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