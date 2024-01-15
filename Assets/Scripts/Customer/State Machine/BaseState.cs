using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor.MPE;
using UnityEditor.Rendering;
using UnityEngine;

namespace LHC.Customer.StateMachine
{
    public abstract class BaseState : IState
    {
        protected Customer m_Customer;
        protected CustomerData m_CustomerData;

        public BaseState(Customer controller, CustomerData customerData)
        {
            m_Customer = controller;
            m_CustomerData = customerData;
        }

        public abstract void OnEnter();

        public abstract void OnExit();

        public abstract void OnTick();

        public void SwitchState(IState newState)
        {
            m_Customer.m_CustomerData.currentState.OnExit();
            m_Customer.m_CustomerData.currentState = newState;
            m_Customer.m_CustomerData.currentState.OnEnter();
        }


        public delegate void OnMovedToPosition();
        protected void MoveTo(Vector3 position, float speed, OnMovedToPosition after = null)
        {
            if (HasReachedTargetPosition(position))
            {
                after?.Invoke();
                return;
            }

            var motion = ( position - m_Customer.transform.position ).normalized;
            m_Customer.CharacterController.Move(motion * speed * Time.deltaTime);
        }

        public delegate void OnRotationFinish();
        protected void LookAt(Quaternion rotation, float speed, OnRotationFinish after = null)
        {
            if ( m_Customer.transform.rotation == rotation )
            {
                after?.Invoke();
            }

            m_Customer.transform.rotation = Quaternion.Slerp(m_Customer.transform.rotation, rotation, speed * Time.deltaTime);
        }

        protected void MoveAndLookTowards(Vector3 position, Quaternion rotation, float moveSpeed, float rotateSpeed, OnMovedToPosition onMove = null, OnRotationFinish onRotate = null)
        {
            LookAt(rotation, rotateSpeed, onRotate);
            MoveTo(position, moveSpeed, onMove);
        }

        protected IEnumerator NavMeshMoveTo(Vector3 position, float speed, int tick = 3, OnMovedToPosition after = null) 
        {
            var t = tick;
            m_Customer.NavController.ResetPath();
            m_Customer.NavController.SetDestination(position);
            m_Customer.NavController.speed = speed;

            while (--t > 0) {
                yield return null;
            }

            while (m_Customer.NavController.remainingDistance >= .2f) {
                //LookAt(Quaternion.LookRotation(m_Customer.NavController.velocity - m_Customer.transform.position).normalized, m_CustomerData.locomotionData.rotationSpeed);
                LookAt(Quaternion.LookRotation(position - m_Customer.transform.position).normalized, m_CustomerData.locomotionData.rotationSpeed);
                Debug.Log(m_Customer.NavController.remainingDistance);
                yield return null;
            }

            after?.Invoke();
        }

        //protected IEnumerator MoveToCoroutine(Vector3 position, float speed, bool orientRotationToMovement = false, OnMovedToPosition after = null)
        public struct MoveConfig {
            public Vector3 position;
            public float moveSpeed;
            public bool orientRotationToMovement;
            public float rotateSpeed;
            public OnMovedToPosition onMoveCallback;
            public OnRotationFinish onRotationFinish;
        }

        protected IEnumerator MoveToCoroutine(MoveConfig settings)
        {
            while (!HasReachedTargetPosition(settings.position))
            {
                if (settings.orientRotationToMovement) {
                    MoveAndLookTowards(settings.position, Quaternion.LookRotation(settings.position - m_Customer.transform.position).normalized,
                    settings.moveSpeed,
                    settings.rotateSpeed,
                    settings.onMoveCallback,
                    settings.onRotationFinish
                    );
                } else {
                    MoveTo(settings.position, settings.moveSpeed, settings.onMoveCallback);
                }
                yield return null;
            }
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
                currentWayPointFinalPos.y = m_Customer.transform.position.y;

                // ROTATE TOWARDS THE WAYPOINT
                var turnRotation = GetDirectionWayPoint(currentWayPoint);

                while (Vector3.SqrMagnitude(m_Customer.transform.position - currentWayPointFinalPos) > 0.3f * 0.3f)
                {
                    LookAt(turnRotation, m_CustomerData.locomotionData.rotationSpeed);
                    MoveTo( currentWayPointFinalPos, m_CustomerData.locomotionData.walkSpeed );
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
            var dirToWaypoint = (wayPoint.transform.position - m_Customer.transform.position).normalized;
            var targetAngle = 90 - Mathf.Atan2(dirToWaypoint.z, dirToWaypoint.x) * Mathf.Rad2Deg;
            return Quaternion.Euler(Vector3.up * targetAngle);
        }

        protected float GetDirectionWayPointAngle(WayPoint wayPoint) 
        {
            var dirToWaypoint = (wayPoint.transform.position - m_Customer.transform.position).normalized;
            var targetAngle = 90 - Mathf.Atan2(dirToWaypoint.z, dirToWaypoint.x) * Mathf.Rad2Deg;
            return targetAngle;
        }

        protected bool HasReachedTargetPosition( Vector3 targetPos )
        {
            return Vector3.SqrMagnitude( m_Customer.transform.position - targetPos ) < 0.2f * 0.2f;
        }
    }
}