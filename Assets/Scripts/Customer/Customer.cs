using UnityEngine;
using LHC.Customer.StateMachine;
using System;
using UnityEngine.Serialization;

namespace LHC.Customer
{
    using LHC.Tavern;
    
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

        // ANIMATION 
        private CustomerAnimationManager m_AnimationManager;

        private CustomerOrderManager m_OrderManager;
        
        // DEBUGGING (Remove this queue to a manager class afterwards)
        public Transform m_DebugSpawnLocation;

        // GETTERS
        public CustomerData GetCustomerData() { return m_CustomerData; }
        public CustomerAnimationManager GetAnimationManager() => m_AnimationManager;
        public CustomerOrderManager GetOrderManager() { return m_OrderManager; }

        private void Awake()
        {
            CharacterController = GetComponent<CharacterController>();
            m_AnimationManager = GetComponent<CustomerAnimationManager>();
            m_OrderManager = GetComponent<CustomerOrderManager>();

            // State instantiation
            IdleState = new IdleState( this, m_CustomerData );
            WanderState = new WanderState( this, m_CustomerData );
            ApproachShopState = new ApproachShopState( this, m_CustomerData );
            OrderState = new OrderFoodState( this, m_CustomerData );
            EatState = new EatState(this, m_CustomerData);
        }

        private void Start()
        {
            m_CustomerData.currentState = IdleState;
            m_CustomerData.currentState.OnEnter();
        }

        private void Update()
        {
            m_CustomerData.currentState.OnTick();
            
            // DEBUG
            if ( Input.GetKeyDown(KeyCode.Space) ) 
            {
                m_CustomerData.currentState.SwitchState( ApproachShopState );
            }
        }

        public bool HasOrderedIngredient()
        {
            return m_CustomerData.currentIngredient != null;
        }

        private void ApplyGravity()
        {
            if (Physics.Raycast(transform.position, -transform.up * 100f, out RaycastHit hitInfo))
            {
                var newPos = transform.position;
                newPos.y = hitInfo.point.y;
            }
        }

        private void OnDrawGizmos() 
        {
            Gizmos.DrawWireSphere( transform.position, m_CustomerData.locomotionData.wanderRadius );
        }

        public Vector3 GetInteractionPosition()
        {
            return this.transform.position;
        }

        private void OnEnable()
        {
        }

        private void OnDisable()
        {
        }
    }
}