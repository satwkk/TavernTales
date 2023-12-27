using System;
using System.Collections;
using LHC.Globals;
using UnityEngine;

namespace LHC.Customer.StateMachine {
    public struct WanderData
    {
        public float wanderSpeed;
        public float wanderRadius;
    }

    public class WanderState : BaseState
    {
        private WanderData m_WanderData;

        public WanderState( Customer controller, CustomerData customerData ) : base( controller, customerData )
        {
            m_WanderData = new WanderData();
            m_WanderData.wanderSpeed = 2f;
            m_WanderData.wanderRadius = 10f;
        }

        public override void OnEnter()
        {
            Debug.Log( "Entering wander state" );
            var randomPos = UnityEngine.Random.insideUnitSphere * m_WanderData.wanderRadius;
            var targetPos = m_Customer.transform.position + randomPos;
            targetPos.y = m_Customer.transform.position.y;
            if (IsNavigable(targetPos))
            {
                m_Customer.StartCoroutine( StartWandering( targetPos ) );
            }
            else
            {
                SwitchState( m_Customer.m_IdleState );
            }
        }

        private bool IsNavigable(Vector3 coord)
        {
            bool hit = Physics.Raycast( coord + Vector3.up * 10f, Vector3.down * 10000f, out RaycastHit hitInfo );
            if (hit)
            {
                Debug.Log( "Hit something at: " + hitInfo.point );
                if ( !hitInfo.collider.CompareTag( "NotNavigable" ) )
                {
                    return true;
                }
            }
            return false;
        }

        IEnumerator StartWandering(Vector3 targetPos )
        {
            var targetRot = Quaternion.LookRotation( ( targetPos - m_Customer.transform.position ) );
            m_Customer.transform.rotation = targetRot;
            while ( m_Customer.transform.position != targetPos)
            {
                if ( CheckForObstacle() )
                {
                    break;
                }

                m_Customer.transform.position = Vector3.MoveTowards( m_Customer.transform.position, targetPos, 2f * Time.deltaTime );
                yield return null;
            }
            SwitchState( m_Customer.m_IdleState );
        }

        private bool CheckForObstacle()
        {
            if ( Physics.Raycast(m_Customer.transform.position, m_Customer.transform.forward, out RaycastHit hitInfo, 2f) )
            {
                Debug.DrawLine( m_Customer.transform.position, hitInfo.point );
                return true;
            }
            return false;
        }

        public override void OnExit()
        {
        }

        public override void OnTick()
        {
        }
    }
}