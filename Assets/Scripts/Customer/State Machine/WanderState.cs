using UnityEngine;
using System;
using System.Diagnostics;
using UnityEngine.AI;

namespace LHC.Customer.StateMachine 
{
    public class WanderState : BaseState
    {
        public Vector3 WanderTargetPos { get; private set; }
        public bool isMoving = false;

        public WanderState( Customer controller, CustomerData customerData ) : base( controller, customerData )
        {
        }

        public override void OnEnter()
        {
            m_Customer.AnimationManager.PlayWalkingAnimation(true);
            WanderTargetPos = GetRandomCoordInAgentRadius();
            m_Customer.NavController.SetDestination(WanderTargetPos);
            m_Customer.NavController.speed = m_CustomerData.locomotionData.walkSpeed;
            isMoving = true;
        }

        public override void OnTick()
        {
            // IF THE AGENT IS NOT MOVING RETURN
            if (!isMoving)
                return;

            // IF THE AGENT HAS REACHED THE DESTINATION THEN GO BACK TO IDLE
            if (m_Customer.transform.position == WanderTargetPos) 
                SwitchState(m_Customer.IdleState);

            // LOOK TOWARDS THE TARGET YOU ARE MOVING TO
            LookAt(Quaternion.LookRotation(WanderTargetPos - m_Customer.transform.position).normalized, m_CustomerData.locomotionData.rotationSpeed);
        }

        public override void OnExit()
        {
            WanderTargetPos = Vector3.zero;
            m_Customer.NavController.isStopped = true;
            m_Customer.NavController.ResetPath();
            m_Customer.AnimationManager.PlayWalkingAnimation(false);
        }

        private Vector3 GetRandomCoordInAgentRadius()
        {
            var randomPos = UnityEngine.Random.insideUnitSphere * m_CustomerData.locomotionData.wanderRadius;
            var targetPos = m_Customer.transform.position + randomPos;
            targetPos.y = m_Customer.transform.position.y;
            return targetPos;
        }

        private bool IsNavigable(Vector3 coord)
        {
            bool hit = Physics.Raycast( coord + Vector3.up * 10f, Vector3.down * 10000f, out RaycastHit hitInfo );
            if (hit)
            {
                if (!hitInfo.collider.CompareTag( "NotNavigable" ))
                {
                    return true;
                }
            }
            return false;
        }

        private void CheckForObstacle()
        {
            if ( Physics.Raycast( m_Customer.transform.position, m_Customer.transform.forward, out RaycastHit hitInfo, 2f ) )
            {
                WanderTargetPos = GetRandomCoordInAgentRadius();
            }
        }

        private bool IsInsideVillageRadius(Vector3 coord)
        {
            if (!m_Customer.wanderingRadiusLimitCollider.bounds.Contains(coord))
                return false;
                
            return true;
        }
    }
}