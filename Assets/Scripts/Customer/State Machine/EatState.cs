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
            customer.AnimationManager.OnPickupAttachDurationReached += OnPickupAttachDurationReached; 
            CookedIngredientServingZone.OnCookedIngredientPlaced += OnCookedIngredientPlaced;
        }

        private void OnPickupAttachDurationReached()
        {
            CookedOrder.transform.SetParent(customer.PickupSocket);
            CookedOrder.transform.position = customer.PickupSocket.position;
        }

        private void OnCookedIngredientPlaced(CookedIngredient cookedOrder)
        {
            CookedOrder = cookedOrder;
            customer.AnimationManager.PlayPickupAnimation(true, OnPickupComplete);
        }

        private void OnPickupComplete()
        {
            // TODO: Have all seats stored in a global game manager class which will be a singleton

            // GET THE AVAILABLE TABLE
            SittingTable = Tavern.Instance.GetAvailableEatingTable();

            // GET THE SEAT FROM THE TABLE TO SIT AT
            Seat = SittingTable.GetSeatForCustomer();

            // SET THE TARGET POSITION AND PLAY THE WALKING ANIMATION
            customer.AnimationManager.PlayWalkingAnimation(true);
            TargetPosition = Seat.SeatingOffsetTransform.position;

            customer.StartCoroutine(NavMeshMoveToCoro(TargetPosition, customerData.locomotionData.walkSpeed, 3, OnReachSit));
            
            // AFTER PICKUP COMPLETE UNSUBSCRIBE FROM THE ANIMATION EVENT
            customer.AnimationManager.OnPickupAttachDurationReached -= OnPickupAttachDurationReached; 
            CookedIngredientServingZone.OnCookedIngredientPlaced -= OnCookedIngredientPlaced;
        }

        public override void OnExit()
        {
            customer.NavController.enabled = true;
        }

        public override void OnTick()
        {}

        private void OnReachSit()
        {
            // DISABLE THE NAVMESH TO DISABLE COLLISION WITH THE CHAIR
            customer.NavController.ResetPath();
            customer.NavController.enabled = false;

            // STOP THE WALKING ANIMATION
            customer.AnimationManager.PlayWalkingAnimation(false);
            
            // SIT ON THE CHAIR
            Seat.Sit(customer, OnSitOnSeat);
        }

        private void OnSitOnSeat()
        {
            // ENABLE THE EATING LAYER 
            customer.AnimationManager.Animator.SetLayerWeight(Constants.EATING_ANIMATION_LAYER, 1f);

            // START EATING TIMER
            customer.StartCoroutine(EatFoodTimer());
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
                customer.AnimationManager.Animator.SetTrigger(Constants.EATING_ANIMATION_TRIGGER_CONDITION);
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
            Object.Destroy(CookedOrder.OriginalIngredient);
            Object.Destroy(CookedOrder.OriginalIngredient.gameObject);
            Object.Destroy(CookedOrder);
            Object.Destroy(CookedOrder.gameObject);

            // STOP THE FOOD HOLD ANIMATION
            customer.AnimationManager.Animator.SetLayerWeight(Constants.EATING_ANIMATION_LAYER, 0f);
            customer.AnimationManager.Animator.SetLayerWeight(Constants.PICKUP_ANIMATION_LAYER, 0f);
            customer.AnimationManager.Animator.SetBool(Constants.PICKUP_ANIMATION_TRIGGER_CONDITION, false);

            // STAND UP
            customer.AnimationManager.Animator.SetBool(Constants.SITTING_ANIMATION_TRIGGER_CONDITION, false);

            // LEAVE THE SHOP
            SwitchState(customer.LeaveState);
        }
    }
}