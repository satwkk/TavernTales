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
        
        public bool HasPickedUpFood { get; private set; } = false;
        
        public CookedIngredient CookedOrder;
        
        public EatState(Customer controller, CustomerData customerData) : base(controller, customerData)
        {
        }

        public override void OnEnter()
        {
            m_Customer.AnimationManager.OnPickupAttachDurationReached += OnPickupAttachDurationReached; 
            CookedIngredientServingZone.OnCookedIngredientPlaced += OnCookedIngredientPlaced;
        }

        private void OnPickupAttachDurationReached()
        {
            CookedOrder.transform.SetParent(m_Customer.PickupSocket);
            CookedOrder.transform.position = m_Customer.PickupSocket.position;
        }

        private void OnCookedIngredientPlaced(CookedIngredient cookedOrder)
        {
            CookedOrder = cookedOrder;
            m_Customer.AnimationManager.PlayPickupAnimation(true, OnPickupComplete);
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
            m_Customer.AnimationManager.PlayWalkingAnimation(true);

            // NOTE: IGNORE COLLISION WHILE GOING TO THE SEAT SINCE WE DO NOT HAVE ANY SENSOR FOR OBSTACLE AVOIDANCE FOR NOW
            Physics.IgnoreCollision(m_Customer.CharacterController, Seat.gameObject.GetComponent<Collider>());
            Physics.IgnoreCollision(m_Customer.CharacterController, SittingTable.transform.Find("Desk").gameObject.GetComponent<Collider>());

            HasPickedUpFood = true;
            
            // AFTER PICKUP COMPLETE UNSUBSCRIBE FROM THE ANIMATION EVENT
            m_Customer.AnimationManager.OnPickupAttachDurationReached -= OnPickupAttachDurationReached; 
        }

        public override void OnExit()
        {
        }

        public override void OnTick()
        {
            // IF PLAYER HAS NOT PLAYED THE PICKUP ANIMATION DO NOT MOVE HIM TO THE TABLE
            if (!HasPickedUpFood) 
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
            m_Customer.AnimationManager.PlayWalkingAnimation(false);
            
            // SIT ON THE CHAIR
            Seat.Sit(m_Customer);

            // START EATING TIMER
            // PLAY EAT ANIMATION IN FREQUENT INTERVALS
            // CHECK IF THERE ARE CUSTOMERS IN THE SITTING TABLE
            // IF YES 
            // LOOK AT THEM WITH YOUR HEAD (LOOK AT IK)
        }
    }
}

