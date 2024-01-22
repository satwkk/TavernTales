using LHC.Tavern;
using UI.Customer;
using UnityEngine;

namespace LHC.Customer.StateMachine
{
    public class OrderFoodState : BaseState
    {
        private bool IsFoodOrdered { get; set; }
        private float CurrentWaitTimer { get; set; } = 0f;
        private Ingredient ingredientToOrder;
        private FoodOrderUI foodOrderUI;

        public OrderFoodState(Customer controller, CustomerData customerData, FoodOrderUI foodOrderUI) : base( controller, customerData )
        {
            this.foodOrderUI = foodOrderUI;
        }

        public override void OnEnter()
        {
            customer.AnimationManager.PlayWalkingAnimation(true);
            customer.StartCoroutine(
                NavMeshMoveToCoro(
                    OrderingTable.Instance.orderingLocationTransform.position, 
                    customerData.locomotionData.walkSpeed, 
                    3, 
                    OnReachOrderingTable
                )
            );
        }

        public override void OnExit()
        {
            IsFoodOrdered = false;
            ingredientToOrder = null;
            CurrentWaitTimer = 0f;
            customer.AnimationManager.PlayWalkingAnimation(false);
        }

        private void OnReachOrderingTable()
        {
            ingredientToOrder = IngredientSpawner.Instance.CreateIngredient();
            ingredientToOrder.OnServe += OnServe;
            foodOrderUI.CraftUI(ingredientToOrder);
            foodOrderUI.Show();
            customer.AnimationManager.PlayWalkingAnimation(false);
            customer.OrderManager.OrderFood(ingredientToOrder);
            IsFoodOrdered = true;
        }

        private void OnServe()
        {
            Debug.Log("Food has been served");
            foodOrderUI.Hide();
            ingredientToOrder.OnServe -= OnServe;
            SwitchState(customer.EatState);
        }

        public override void OnTick()
        {
            if (!IsFoodOrdered)
                return;

            if (CurrentWaitTimer >= ingredientToOrder.IngredientData.CustomerWaitingTimer && ingredientToOrder.IsServed.Equals(false))
            {
                Debug.LogError("Customer has waited long enough and no food has been served");
                foodOrderUI.Hide();
                ingredientToOrder.OnServe -= OnServe;
                SwitchState(customer.AngryState);
            }

            CurrentWaitTimer += Time.deltaTime;
        }
    }
}
