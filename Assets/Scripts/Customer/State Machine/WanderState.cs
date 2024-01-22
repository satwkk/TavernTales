using UnityEngine;
using UnityEngine.AI;

namespace LHC.Customer.StateMachine 
{
    public class WanderState : BaseState
    {
        public Vector3 wanderTargetPos;
        private bool isMoving = false;

        public WanderState( Customer controller, CustomerData customerData ) : base( controller, customerData )
        {
        }

        public override void OnEnter()
        {
            customer.AnimationManager.PlayWalkingAnimation(true);
            wanderTargetPos = GetRandomCoordInAgentRadius();
            bool canReach = NavMesh.SamplePosition(wanderTargetPos, out NavMeshHit hit, 20.0f, 1);
            Debug.Log("Can Reach: " + canReach);

            if (IsNavigable(wanderTargetPos)) 
            {
                customer.NavController.SetDestination(wanderTargetPos);
                customer.NavController.speed = customerData.locomotionData.walkSpeed;
                isMoving = true;
            }
            else 
            {
                UnityEngine.Debug.LogError("Going back to idle");
                SwitchState(customer.IdleState);
            }
        }

        public override void OnTick()
        {
            // IF THE AGENT IS NOT MOVING RETURN
            if (!isMoving)
                return;

            if (ObstacleInPath() || HasReachedTargetPosition(wanderTargetPos))
            {
                isMoving = false;
                SwitchState(customer.IdleState);
            }

            // IF THE AGENT HAS REACHED THE DESTINATION THEN GO BACK TO IDLE
            // if (customer.transform.position == wanderTargetPos) 
            // {
            //     isMoving = false;
            //     SwitchState(customer.IdleState);
            // }

            // LOOK TOWARDS THE TARGET YOU ARE MOVING TO
            LookAt(Quaternion.LookRotation(wanderTargetPos - customer.transform.position).normalized, customerData.locomotionData.rotationSpeed);
        }

        public override void OnExit()
        {
            wanderTargetPos = Vector3.zero;
            customer.NavController.isStopped = true;
            customer.NavController.ResetPath();
            customer.AnimationManager.PlayWalkingAnimation(false);
        }

        private Vector3 GetRandomCoordInAgentRadius()
        {
            var randomPos = UnityEngine.Random.insideUnitSphere * customerData.locomotionData.wanderRadius;
            var targetPos = customer.transform.position + randomPos;
            targetPos.y = customer.transform.position.y;
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

        private bool ObstacleInPath()
        {
            // if ( Physics.Raycast( customer.transform.position, customer.transform.forward, out RaycastHit hitInfo, 2f ) )
            if (Physics.SphereCast(customer.transform.position, customer.NavController.radius, customer.transform.forward, out RaycastHit hitInfo, 2.0f))
            {
                return true;
            }
            return false;
        }
    }
}