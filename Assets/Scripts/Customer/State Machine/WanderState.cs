using UnityEngine;

namespace LHC.Customer.StateMachine {

    public class WanderState : BaseState
    {
        private Vector3 m_WanderTargetPos;
        private bool m_CanWander = false;

        public WanderState( Customer controller, CustomerData customerData ) : base( controller, customerData )
        {
        }

        public override void OnEnter()
        {
            Debug.Log( "Entering wander state" );
            m_Customer.AnimationManager.PlayWalkingAnimation(true);
            m_WanderTargetPos = GetRandomCoordInAgentRadius();
            m_CanWander = IsNavigable(m_WanderTargetPos);

            // If the random coord in not navigable simply go back to idle state and try again
            if (!m_CanWander)
            {
                SwitchState(m_Customer.IdleState);
            }
        }

        public override void OnTick()
        {
            if (m_CanWander)
            {
                CheckForObstacle();
                LookAt(Quaternion.LookRotation(m_WanderTargetPos - m_Customer.transform.position), m_CustomerData.locomotionData.rotationSpeed);
                MoveTo(m_WanderTargetPos, m_CustomerData.locomotionData.walkSpeed, () => {
                    SwitchState(m_Customer.IdleState);
                });
            }
        }

        public override void OnExit()
        {
            m_CanWander = false;
            m_WanderTargetPos = Vector3.zero;
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
                Debug.Log( "Hit something at: " + hitInfo.point );
                if ( !hitInfo.collider.CompareTag( "NotNavigable" ) )
                {
                    return true;
                }
            }
            Debug.Log( "We hit something that is not navigable" );
            return false;
        }

        private void CheckForObstacle()
        {
            if ( Physics.Raycast( m_Customer.transform.position, m_Customer.transform.forward, out RaycastHit hitInfo, 2f ) )
            {
                m_CanWander = false;
                SwitchState(m_Customer.IdleState);
            }
        }
    }
}