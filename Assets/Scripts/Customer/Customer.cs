using UnityEngine;
using LHC.Customer.StateMachine;
using System;
using LHC.Globals;
using TMPro;
using UI.Customer;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace LHC.Customer
{
    using LHC.Tavern;
    using Unity.AI.Navigation.Editor;
    using Unity.Jobs;
    using UnityEditor;
    using UnityEngine.AI;

    public enum CustomerType
    {
        CIVILIAN,
        KNIGHT
    }

    public enum CustomerMood
    {
        HAPPY,
        ANGRY,
        FRUSTRATED
    }

    [Serializable]
    public class LocomotionData
    {
        public float walkSpeed;
        public float wanderRadius;
        public float rotationSpeed;
    }

    [Serializable]
    public class InteractionData
    {
        public Transform interactionRaycastPosition;
        public float interactionRange;
    }

    [Serializable]
    public class CustomerData
    {
        public CustomerType customerType;
        public CustomerMood customerMood;
        public LocomotionData locomotionData;
        public InteractionData interactionData;
        public Ingredient currentIngredient;
        public IState currentState;
    }

    [RequireComponent(typeof(CharacterController), typeof(CustomerAnimationManager), typeof(CustomerOrderManager))]
    public class Customer : MonoBehaviour, IInteractionActor
    {
        // DATA CLASS CONTAINING ALL SETTINGS FOR CUSTOMERS
        [SerializeField] private CustomerData customerData;
        
        // FOR BASIC MOVEMENT WHICH REQUIRES NO PATHFINDING
        public CharacterController characterController;
        
        // ========================================= STATES =============================================
        public IdleState IdleState { get; private set; }
        public WanderState WanderState { get; private set; }
        public ApproachShopState ApproachShopState { get; private set; }
        public OrderFoodState OrderState { get; private set; }
        public EatState EatState { get; private set; }
        public AngryState AngryState { get; private set; }
        public LeaveState LeaveState { get; private set; }
        
        public Transform shopEntryPosition;
        public Transform orderingTablePosition;
        // ==============================================================================================

        // ANIMATION MANAGER FOR HANDLING ANIMATIONS FOR CUSTOMER
        public CustomerAnimationManager AnimationManager { get; private set; }
        
        // SOCKET FOR PICKUPS TO ATTACH TO
        public Transform PickupSocket;
        
        // ORDER MANAGER WHICH ORDERS FOOD TO THE TAVERN OWNER
        public CustomerOrderManager OrderManager { get; private set; }

        // CONTROLLER THAT USES PATHFINDING FOR AI TRAVERSAL AROUND THE LEVEL
        public NavMeshAgent NavController;

        // UI FOR FOOD ORDER FEEDBACK
        public FoodOrderUI foodOrderUI;

        // NOTE: DEBUGGING VARIABLES (REMOVE IN BUILD)
        public TextMeshProUGUI m_TextMesh;
        
        // ========================================= GETTERS ==================================================
        
        public CustomerData GetCustomerData() { return customerData; }
        
        // =====================================================================================================

        private void Awake()
        {
            shopEntryPosition = GameObject.Find(Constants.SHOP_ENTRY_SOCKET_DEBUG).transform;
            orderingTablePosition = GameObject.Find(Constants.ORDERING_ZONE_SOCKET).transform;

            NavController = GetComponent<NavMeshAgent>();
            characterController = GetComponent<CharacterController>();
            AnimationManager = GetComponent<CustomerAnimationManager>();
            OrderManager = GetComponent<CustomerOrderManager>();

            // State instantiation
            IdleState = new IdleState( this, customerData );
            WanderState = new WanderState( this, customerData );
            ApproachShopState = new ApproachShopState( this, customerData, shopEntryPosition );
            OrderState = new OrderFoodState( this, customerData, foodOrderUI );
            EatState = new EatState(this, customerData);
            LeaveState = new LeaveState(this, customerData);
            AngryState = new AngryState(this, customerData);
        }

        private void Start()
        {
            customerData.currentState = IdleState;
            customerData.currentState.OnEnter();
        }

        private void Update()
        {
            var tokens = customerData.currentState.ToString().Split('.');
            m_TextMesh.text = tokens[^1];
            customerData.currentState.OnTick();
        }

        // DEBUG
        public void SwitchStateDebug()
        {
            customerData.currentState.SwitchState( ApproachShopState );
        }

        private void OnDrawGizmos() 
        {
            // Gizmos.DrawWireSphere( transform.position, m_CustomerData.locomotionData.wanderRadius );
            if (WanderState == null)
                return;
            Handles.DrawWireCube(WanderState.wanderTargetPos, Vector3.one);
        }

        public Vector3 GetInteractionPosition()
        {
            return this.transform.position;
        }
    }
}