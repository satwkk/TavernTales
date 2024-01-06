using UnityEngine;
using LHC.Customer.StateMachine;
using System;

namespace LHC.Customer
{
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
        public CharacterController m_CharacterController;

        // STATES
        public IdleState m_IdleState;
        public WanderState m_WanderState;
        public ApproachShopState m_ApproachShopState;
        public OrderFoodState m_OrderState;

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
            m_AnimationManager = GetComponent<CustomerAnimationManager>();
            m_OrderManager = GetComponent<CustomerOrderManager>();

            // State instantiation
            m_IdleState = new IdleState( this, m_CustomerData );
            m_WanderState = new WanderState( this, m_CustomerData );
            m_ApproachShopState = new ApproachShopState( this, m_CustomerData );
            m_OrderState = new OrderFoodState( this, m_CustomerData );
        }

        private void Start()
        {
            m_CustomerData.currentState = m_IdleState;
            m_CustomerData.currentState.OnEnter();
        }

        private void Update()
        {
            m_CustomerData.currentState.OnTick();
            
            // DEBUG
            if ( Input.GetKeyDown(KeyCode.Space) ) 
            {
                m_CustomerData.currentState.SwitchState( m_ApproachShopState );
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