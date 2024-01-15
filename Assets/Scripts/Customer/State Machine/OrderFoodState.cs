using LHC.Tavern;
using UnityEngine;

namespace LHC.Customer.StateMachine
{
    public class OrderFoodState : BaseState
    {
        public bool IsFoodOrdered { get; set; }
        public Ingredient ingredientToOrder;
        public Ingredient IngredientToOrder { get => ingredientToOrder; set => ingredientToOrder = value; }
        public float CurrentWaitTimer { get; private set; } = 0f;

        public OrderFoodState(Customer controller, CustomerData customerData) : base( controller, customerData )
        {
        }

        public override void OnEnter()
        {
            IngredientToOrder = IngredientSpawner.Instance.CreateIngredient();
            IngredientToOrder.IngredientData.OnServe += OnServe;
            m_Customer.AnimationManager.PlayWalkingAnimation(true);
            m_Customer.StartCoroutine(NavMeshMoveTo(OrderingTable.Instance.OrderingPosition, m_CustomerData.locomotionData.walkSpeed, 3, OnReachOrderingTable));
        }

        public override void OnExit()
        {
            IsFoodOrdered = false;
            IngredientToOrder = null;
            CurrentWaitTimer = 0f;
            m_Customer.AnimationManager.PlayWalkingAnimation(false);
        }

        private void OnReachOrderingTable() {
            m_Customer.AnimationManager.PlayWalkingAnimation(false);
            m_Customer.OrderManager.OrderFood(ingredientToOrder);
            IsFoodOrdered = true;
        }

        private void OnServe()
        {
            Debug.Log("Food has been served");
            IngredientToOrder.IngredientData.OnServe -= OnServe;
            SwitchState(m_Customer.EatState);
        }

        public override void OnTick()
        {
            if (!IsFoodOrdered)
                return;

            if (CurrentWaitTimer >= IngredientToOrder.IngredientData.CustomerWaitingTimer && IngredientToOrder.IngredientData.IsServed.Equals(false))
            {
                Debug.LogError("Customer has waited long enough and no food has been served");
                IngredientToOrder.IngredientData.OnServe -= OnServe;
                SwitchState(m_Customer.AngryState);
            }

            CurrentWaitTimer += Time.deltaTime;
        }
    }
}
