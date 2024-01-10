using UnityEngine;

namespace LHC.Customer.StateMachine {

    public class WanderState : BaseState
    {
        public Vector3 WanderTargetPos { get; private set; }
        public bool CanWander { get; private set; } = false;
        public Vector3 RotatingVector;
        public float CurrentYawRotation;
        public float RotateAmount = 0.1f;

        public WanderState( Customer controller, CustomerData customerData ) : base( controller, customerData )
        {
        }

        public override void OnEnter()
        {
            Debug.Log( "Entering wander state" );
            m_Customer.AnimationManager.PlayWalkingAnimation(true);
            WanderTargetPos = GetRandomCoordInAgentRadius();
            // CanWander = IsNavigable(WanderTargetPos);

            // If the random coord in not navigable simply go back to idle state and try again
            // if (!CanWander)
            // {
            //     SwitchState(m_Customer.IdleState);
            // }
        }

        public override void OnTick()
        {
            CheckForObstacle();
            LookAt(Quaternion.LookRotation(WanderTargetPos - m_Customer.transform.position), m_CustomerData.locomotionData.rotationSpeed);
            MoveTo(WanderTargetPos, m_CustomerData.locomotionData.walkSpeed, () => {
                SwitchState(m_Customer.IdleState);
            });
            // if (CanWander)
            // {
            // }
        }

        public override void OnExit()
        {
            CanWander = false;
            WanderTargetPos = Vector3.zero;
            RotatingVector = Vector3.zero;
            CurrentYawRotation = 0f;
            m_Customer.AnimationManager.PlayWalkingAnimation(false);
        }

        private Vector3 GetRandomCoordInAgentRadius()
        {
            var randomPos = Random.insideUnitSphere * m_CustomerData.locomotionData.wanderRadius;
            var targetPos = m_Customer.transform.position + randomPos;
            targetPos.y = m_Customer.transform.position.y;
            return targetPos;
        }

        private bool IsNavigable(Vector3 coord)
        {
            bool hit = Physics.Raycast( coord + Vector3.up * 10f, Vector3.down * 10000f, out RaycastHit hitInfo );
            if (hit)
            {
                if ( !hitInfo.collider.CompareTag( "NotNavigable" ) )
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
                // CanWander = false;
                // SwitchState(m_Customer.IdleState);
            }
        }
    }
}