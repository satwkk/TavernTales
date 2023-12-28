using LHC.Globals;
using UnityEngine;
using UnityEngine.AI;
using LHC.Customer.StateMachine;

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
    public struct CustomerData
    {
        public CustomerType customerType;
        public CustomerMood CustomerMood;
        public IState currentState;
    }

    public class Customer : MonoBehaviour
    {
        public CustomerData m_CustomerData;
        public IdleState m_IdleState;
        public WanderState m_WanderState;
        public ApproachShopState m_ApproachShopState;
        public OrderFoodState m_OrderState;

        // GETTERS
        public CustomerData GetCustomerData() { return m_CustomerData; }

        private void Awake()
        {
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
        }
    }
}