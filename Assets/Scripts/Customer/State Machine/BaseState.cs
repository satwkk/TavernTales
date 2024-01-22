using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace LHC.Customer.StateMachine
{
    public abstract class BaseState : IState
    {
        protected Customer customer;
        protected CustomerData customerData;

        public BaseState(Customer controller, CustomerData customerData)
        {
            customer = controller;
            this.customerData = customerData;
        }

        public abstract void OnEnter();

        public abstract void OnExit();

        public abstract void OnTick();

        public void SwitchState(IState newState)
        {
            customer.GetCustomerData().currentState.OnExit();
            customer.GetCustomerData().currentState = newState;
            customer.GetCustomerData().currentState.OnEnter();
        }

        public delegate void OnMovedToPosition();
        protected void MoveTo(Vector3 position, float speed, OnMovedToPosition after = null)
        {
            if (HasReachedTargetPosition(position))
            {
                after?.Invoke();
                return;
            }

            var motion = ( position - customer.transform.position ).normalized;
            customer.characterController.Move(motion * speed * Time.deltaTime);
        }

        public delegate void OnRotationFinish();
        protected void LookAt(Quaternion rotation, float speed, OnRotationFinish after = null)
        {
            if ( customer.transform.rotation == rotation )
            {
                after?.Invoke();
            }

            customer.transform.rotation = Quaternion.Slerp(customer.transform.rotation, rotation, speed * Time.deltaTime);
        }

        protected void MoveAndLookTowards(Vector3 position, Quaternion rotation, float moveSpeed, float rotateSpeed, OnMovedToPosition onMove = null, OnRotationFinish onRotate = null)
        {
            LookAt(rotation, rotateSpeed, onRotate);
            MoveTo(position, moveSpeed, onMove);
        }

        protected IEnumerator NavMeshMoveToCoro(Vector3 position, float speed, int tick = 3, OnMovedToPosition after = null) 
        {
            var t = tick;
            customer.NavController.ResetPath();
            customer.NavController.SetDestination(position);
            customer.NavController.speed = speed;

            while (--t > 0) {
                yield return null;
            }

            while (customer.NavController.remainingDistance >= .2f) {
                //LookAt(Quaternion.LookRotation(m_Customer.NavController.velocity - m_Customer.transform.position).normalized, m_CustomerData.locomotionData.rotationSpeed);
                LookAt(Quaternion.LookRotation(position - customer.transform.position).normalized, customerData.locomotionData.rotationSpeed);
                Debug.Log(customer.NavController.remainingDistance);
                yield return null;
            }

            after?.Invoke();
        }

        protected IEnumerator NavMeshFollowWayPoints(List<WayPoint> wayPoints, float speed, int tick = 3, OnMovedToPosition after = null) 
        {
            var currentIndex = 0;
            while (currentIndex < wayPoints.Count)
            {
                var wayPoint = wayPoints[currentIndex++];
                var t = tick;
                customer.NavController.destination = wayPoint.transform.position;
                customer.NavController.speed = speed;

                // Delay before checking for remainingDistance as per navmeshagent docs
                while (--t > 0) {
                    yield return null;
                }

                while (customer.NavController.remainingDistance >= .2f) {
                    // LookAt(Quaternion.LookRotation(wayPoint.transform.position - customer.transform.position).normalized, customerData.locomotionData.rotationSpeed);
                    yield return null;
                }

                // A millisecond delay before setting the destination again
                yield return new WaitForSeconds(.1f);
            }

            after?.Invoke();
            yield return null;
        }

        protected IEnumerator FollowWayPoints(List<WayPoint> wayPoints, Action after = null) 
        {
            int currentIndex = 0;
            WayPoint currentWayPoint;
            while (currentIndex < wayPoints.Count)
            {
                currentWayPoint = wayPoints[currentIndex];

                // MODIFY THE Y AXIS TO ALIGN WITH PLAYER OR ELSE THE PLAYER WILL START WALKING IN AIR
                var currentWayPointFinalPos = currentWayPoint.transform.position;
                currentWayPointFinalPos.y = customer.transform.position.y;

                // ROTATE TOWARDS THE WAYPOINT
                var turnRotation = GetDirectionWayPoint(currentWayPoint);

                while (Vector3.SqrMagnitude(customer.transform.position - currentWayPointFinalPos) > 0.3f * 0.3f)
                {
                    LookAt(turnRotation, customerData.locomotionData.rotationSpeed);
                    MoveTo( currentWayPointFinalPos, customerData.locomotionData.walkSpeed );
                    yield return null;
                }
                currentIndex++;
            }

            // CALLBACK AFTER CUSTOMER HAS REACHED THE FINAL WAYPOINT 
            Debug.Log("Reached the last waypoint");
            after?.Invoke();
            yield return null;
        }

        protected Quaternion GetDirectionWayPoint(WayPoint wayPoint) 
        {
            var dirToWaypoint = (wayPoint.transform.position - customer.transform.position).normalized;
            var targetAngle = 90 - Mathf.Atan2(dirToWaypoint.z, dirToWaypoint.x) * Mathf.Rad2Deg;
            return Quaternion.Euler(Vector3.up * targetAngle);
        }

        protected float GetDirectionWayPointAngle(WayPoint wayPoint) 
        {
            var dirToWaypoint = (wayPoint.transform.position - customer.transform.position).normalized;
            var targetAngle = 90 - Mathf.Atan2(dirToWaypoint.z, dirToWaypoint.x) * Mathf.Rad2Deg;
            return targetAngle;
        }

        protected bool HasReachedTargetPosition( Vector3 targetPos )
        {
            return customer.NavController.remainingDistance <= .2f;
            // return Vector3.SqrMagnitude( customer.transform.position - targetPos ) < 0.2f * 0.2f;
        }
    }
}