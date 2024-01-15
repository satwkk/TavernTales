using UnityEngine;
using LHC.Customer.StateMachine;
using System;
using UnityEngine.Serialization;

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
        public CustomerData m_CustomerData;

        // LOCOMOTION SHIT
        public CharacterController CharacterController { get; private set; }

        // STATES
        public IdleState IdleState { get; private set; }

        public WanderState WanderState { get; private set; }

        public Transform shopEntryPosition;
        public ApproachShopState ApproachShopState { get; private set; }
        
        public Transform orderingTablePosition;
        public OrderFoodState OrderState { get; private set; }

        public EatState EatState { get; private set; }

        public AngryState AngryState { get; private set; }
        public LeaveState LeaveState { get; private set; }

        // ANIMATION 
        public CustomerAnimationManager AnimationManager { get; private set; }
        
        // PICKUP
        public Transform PickupSocket;
        
        public CustomerOrderManager OrderManager { get; private set; }

        public BoxCollider wanderingRadiusLimitCollider;

        public NavMeshAgent NavController;
        
        // GETTERS
        public CustomerData GetCustomerData() { return m_CustomerData; }

        private void Awake()
        {
            shopEntryPosition = GameObject.Find("Shop Entry").GetComponent<Transform>();
            NavController = GetComponent<NavMeshAgent>();
            CharacterController = GetComponent<CharacterController>();
            AnimationManager = GetComponent<CustomerAnimationManager>();
            OrderManager = GetComponent<CustomerOrderManager>();

            // State instantiation
            IdleState = new IdleState( this, m_CustomerData );
            WanderState = new WanderState( this, m_CustomerData );
            ApproachShopState = new ApproachShopState( this, m_CustomerData, shopEntryPosition );
            OrderState = new OrderFoodState( this, m_CustomerData );
            EatState = new EatState(this, m_CustomerData);
            LeaveState = new LeaveState(this, m_CustomerData);
            AngryState = new AngryState(this, m_CustomerData);
        }

        private void Start()
        {
            m_CustomerData.currentState = IdleState;
            m_CustomerData.currentState.OnEnter();
        }

        private void Update()
        {
            m_CustomerData.currentState.OnTick();
        }

        // DEBUG
        public void SwitchStateDebug()
        {
            m_CustomerData.currentState.SwitchState( ApproachShopState );
        }

        private void OnDrawGizmos() 
        {
            Gizmos.DrawWireSphere( transform.position, m_CustomerData.locomotionData.wanderRadius );
            if (WanderState == null)
                return;
            Handles.DrawWireCube(WanderState.WanderTargetPos, Vector3.one);
        }

        public Vector3 GetInteractionPosition()
        {
            return this.transform.position;
        }
    }
}