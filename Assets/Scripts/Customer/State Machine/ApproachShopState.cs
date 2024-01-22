using System.Collections;
using UnityEditor.Rendering;
using UnityEngine;

namespace LHC.Customer.StateMachine
{
    public class ApproachShopState : BaseState
    {
        public Vector3 target;
        public int currentIndex;
        public bool bIsFollowingPath;
        private WayPoint currentWaypoint;

        public ApproachShopState( Customer controller, CustomerData customerData, Transform shopEntryPosition) : base( controller, customerData )
        {
            target = shopEntryPosition.position;
            bIsFollowingPath = false;
        }

        public override void OnEnter()
        {
            // GET THE WAYPOINTS
            customer.AnimationManager.PlayWalkingAnimation(true);

            // m_Customer.StartCoroutine(NavMeshMoveToCoro(this.target, this.m_CustomerData.locomotionData.walkSpeed, 3, after: OnReachShop));

            customer.StartCoroutine(
                NavMeshFollowWayPoints(
                    WayPointManager.instance.approachShopWayPoint, 
                    customerData.locomotionData.walkSpeed, 
                    3, 
                    after: OnReachShop
                )
            );
        }

        public override void OnExit()
        {
            customer.AnimationManager.PlayWalkingAnimation(false);
        }

        public override void OnTick()
        {
        }

        private void OnReachShop() 
        {
            Debug.Log("Reached the target waypoint in shop");
            SwitchState(customer.OrderState);
        }

        private void CheckForShopDoor()
        {
            if (Physics.Raycast(
                customerData.interactionData.interactionRaycastPosition.position, 
                customerData.interactionData.interactionRaycastPosition.forward, 
                out RaycastHit hitInfo, 
                customerData.interactionData.interactionRange
                )
            )
            {
                var door = hitInfo.collider.GetComponent<Door_Tavern>();
                if (door != null && !door.bIsOpen())
                {
                    door.Interact(customer);
                }
            }
        }
    }
}