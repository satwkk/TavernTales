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

        // VARIABLES TO PLAY THE EATING ANIMATION AT RANDOM INTERVALS
        public float RandomEatingTimer;
        public float CurrentEatingTimer;
        
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
            // TODO: Have all seats stored in a global game manager class which will be a singleton

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
            Seat.Sit(m_Customer, OnSitOnSeat);
        }

        private void OnSitOnSeat()
        {
            // ENABLE THE EATING LAYER 
            m_Customer.AnimationManager.Animator.SetLayerWeight(Constants.EATING_ANIMATION_LAYER, 1f);

            // START EATING TIMER
            m_Customer.StartCoroutine(EatFoodTimer());
        }

        private IEnumerator EatFoodTimer()
        {
            var currentEatingTimer = 0;
            var targetEatingTimer = CookedOrder.OriginalIngredient.IngredientData.EatingDuration;
            RandomEatingTimer = Random.Range(3, 6);
            Debug.LogError("Target eating timer: " + targetEatingTimer);
            while (currentEatingTimer < targetEatingTimer)
            {
                Debug.LogError("Current eating timer: " + currentEatingTimer);
                EatFood();
                // TalkToNearByCustomers();

                currentEatingTimer += 1;
                yield return new WaitForSeconds(1f);
            }
            Debug.Log("eating complete");

            yield return new WaitForSeconds(2f);
            CleanUp();
        }

        private void EatFood()
        {
            if (CurrentEatingTimer >= RandomEatingTimer)
            {
                // TODO: This isn't working maybe reset the trigger in a animationstateinfo script or use boolean
                m_Customer.AnimationManager.Animator.SetTrigger(Constants.EATING_ANIMATION_TRIGGER_CONDITION);
                m_Customer.AnimationManager.Animator.ResetTrigger(Constants.EATING_ANIMATION_TRIGGER_CONDITION);
                CurrentEatingTimer = 0f;
                RandomEatingTimer = Random.Range(1, 5);
            }
            else 
            {
                CurrentEatingTimer += 1;
            }
        }

        private void CleanUp()
        {
            Debug.LogWarning("Cleaning up");

            // DESTROY THE COOKED INGREDIENT INSTANCE
            GameObject.Destroy(CookedOrder.gameObject);

            // STOP THE FOOD HOLD ANIMATION
            m_Customer.AnimationManager.Animator.SetLayerWeight(Constants.EATING_ANIMATION_LAYER, 0f);
            m_Customer.AnimationManager.Animator.SetLayerWeight(Constants.PICKUP_ANIMATION_LAYER, 0f);
            m_Customer.AnimationManager.Animator.SetBool(Constants.PICKUP_ANIMATION_TRIGGER_CONDITION, false);

            // STAND UP
            m_Customer.AnimationManager.Animator.SetBool(Constants.SITTING_ANIMATION_TRIGGER_CONDITION, false);

            // LEAVE THE SHOP
            SwitchState(m_Customer.LeaveState);
        }
    }
}

