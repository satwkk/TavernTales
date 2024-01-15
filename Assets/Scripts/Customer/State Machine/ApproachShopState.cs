using System.Collections.Generic;
using UnityEngine;

namespace LHC.Customer.StateMachine
{
    public class ApproachShopState : BaseState
    {
        public Vector3 target;

        public ApproachShopState( Customer controller, CustomerData customerData, Transform shopEntryPosition) : base( controller, customerData )
        {
            this.target = shopEntryPosition.position;
        }

        public override void OnEnter()
        {
            // GET THE WAYPOINTS
            m_Customer.AnimationManager.PlayWalkingAnimation(true);
            m_Customer.StartCoroutine(NavMeshMoveTo(this.target, this.m_CustomerData.locomotionData.walkSpeed, 3, after: OnReachShop));
        }

        public override void OnExit()
        {
            m_Customer.AnimationManager.PlayWalkingAnimation(false);
        }

        public override void OnTick()
        { }

        private void OnReachShop() 
        {
            Debug.Log("Reached the target waypoint in shop");
            SwitchState(m_Customer.OrderState);
        }

        private void CheckForShopDoor()
        {
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