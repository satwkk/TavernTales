using System.Collections;
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
            m_Customer.AnimationManager.PlayWalkingAnimation(true);
            TargetPosition = Seat.SeatingOffsetTransform.position;

            m_Customer.StartCoroutine(NavMeshMoveTo(TargetPosition, m_CustomerData.locomotionData.walkSpeed, 3, OnReachSit));
            
            // AFTER PICKUP COMPLETE UNSUBSCRIBE FROM THE ANIMATION EVENT
            m_Customer.AnimationManager.OnPickupAttachDurationReached -= OnPickupAttachDurationReached; 
        }

        public override void OnExit()
        {
            m_Customer.NavController.enabled = true;
        }

        public override void OnTick()
        {}

        private void OnReachSit()
        {
            // DISABLE THE NAVMESH TO DISABLE COLLISION WITH THE CHAIR
            m_Customer.NavController.ResetPath();
            m_Customer.NavController.enabled = false;

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