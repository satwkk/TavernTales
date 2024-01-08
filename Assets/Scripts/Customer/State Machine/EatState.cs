using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LHC.Customer.StateMachine
{
    using LHC.Globals;
    using LHC.Tavern;

    public class EatState : BaseState
    {
        public EatingTable SittingTable { get; private set; }
        public Seat Seat { get; private set; }
        public Vector3 TargetPosition;
        public bool HasReachedSeat { get; private set; } = false;
        public bool HasPickedupFood { get; private set; } = false;
        
        public EatState( Customer controller, CustomerData customerData ) : base( controller, customerData )
        {
        }

        public override void OnEnter()
        {
            m_Customer.GetAnimationManager().PlayPickupAnimation(true, OnPickupComplete);
        }

        private void OnPickupComplete()
        {
            // GET THE AVAILABLE TABLE
            SittingTable = Tavern.Instance.GetAvailableEatingTable();

            // GET THE SEAT FROM THE TABLE TO SIT AT
            Seat = SittingTable.GetSeatForCustomer();

            // SET THE TARGET POSITION AND PLAY THE WALKING ANIMATION
            TargetPosition = Seat.SeatingOffsetTransform.position;
            TargetPosition.y = m_Customer.transform.position.y;
            m_Customer.GetAnimationManager().PlayWalkingAnimation(true);

            // NOTE: IGNORE COLLISION WHILE GOING TO THE SEAT SINCE WE DO NOT HAVE ANY SENSOR FOR OBSTACLE AVOIDANCE FOR NOW
            Physics.IgnoreCollision(m_Customer.CharacterController, Seat.gameObject.GetComponent<Collider>());
            Physics.IgnoreCollision(m_Customer.CharacterController, SittingTable.transform.Find("Desk").gameObject.GetComponent<Collider>());

            HasPickedupFood = true;
        }

        public override void OnExit()
        {
        }

        public override void OnTick()
        {
            // IF PLAYER HAS NOT PLAYED THE PICKUP ANIMATION DO NOT MOVE HIM TO THE TABLE
            if (!HasPickedupFood) 
                return;
                
            // MOVE TOWARDS THE TABLE AND SIT AT A SEAT
            if (HasReachedSeat)
                return;

            MoveAndLookTowards(
                TargetPosition,
                Quaternion.LookRotation((TargetPosition - m_Customer.transform.position).normalized),
                m_CustomerData.locomotionData.walkSpeed, 
                m_CustomerData.locomotionData.rotationSpeed, 
                OnReachSit
            );
        }

        private void OnReachSit()
        {
            HasReachedSeat = true;

            // STOP THE WALKING ANIMATION
            m_Customer.GetAnimationManager().PlayWalkingAnimation(false);
            
            // SIT ON THE CHAIR
            Seat.Sit(m_Customer);

            // PLAY THE SITTING ANIMATION
            // m_Customer.GetAnimationManager().PlaySittingAnimation(true);
        }
    }
}

