using UnityEngine;
using LHC.Customer.StateMachine;
using System;
using UnityEngine.Serialization;

namespace LHC.Customer
{
    using LHC.Tavern;
    using Unity.Jobs;
    using UnityEditor;

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

    [System.Serializable]
    public struct LocomotionData
    {
        public float walkSpeed;
        public float wanderRadius;
        public float rotationSpeed;
    }

    [System.Serializable]
    public struct InteractionData
    {
        public float interactionRange;
    }

    [System.Serializable]
    public struct CustomerData
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
        public ApproachShopState ApproachShopState { get; private set; }
        public OrderFoodState OrderState { get; private set; }
        public EatState EatState { get; private set; }
        public LeaveState LeaveState { get; private set; }

        // ANIMATION 
        public CustomerAnimationManager AnimationManager { get; private set; }
        
        // PICKUP
        public Transform PickupSocket;
        
        public CustomerOrderManager OrderManager { get; private set; }
        
        // =========================================================== DEBUGGING VARIABLES(Remove this queue to a manager class afterwards)
        public Transform m_DebugSpawnLocation;
        public Transform ObstacleDetectorTransform;
        // =================================================================================================================================

        // GETTERS
        public CustomerData GetCustomerData() { return m_CustomerData; }

        private void Awake()
        {
            CharacterController = GetComponent<CharacterController>();
            AnimationManager = GetComponent<CustomerAnimationManager>();
            OrderManager = GetComponent<CustomerOrderManager>();

            // State instantiation
            IdleState = new IdleState( this, m_CustomerData );
            WanderState = new WanderState( this, m_CustomerData );
            ApproachShopState = new ApproachShopState( this, m_CustomerData );
            OrderState = new OrderFoodState( this, m_CustomerData );
            EatState = new EatState(this, m_CustomerData);
            LeaveState = new LeaveState(this, m_CustomerData);
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