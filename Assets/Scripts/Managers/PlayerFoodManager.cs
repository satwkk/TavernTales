using UnityEngine;

public class PlayerFoodManager : MonoBehaviour {
    [SerializeField] private Food m_Food;

    private void OnEnable() {
        Food.OnFoodPick += OnFoodPick;
        CookingPot.OnFoodAddToPot_Event += OnFoodAddToPot;
    }

    private void OnDisable() {
        Food.OnFoodPick -= OnFoodPick;
        CookingPot.OnFoodAddToPot_Event -= OnFoodAddToPot;
    }

    private void OnFoodAddToPot(Food food) {
        if (m_Food == food) {

            // USE THE FOOD, i.e DISABLE THE OBJECT
            food.Use();

            // SET THE FOOD REFERENCE TO NULL BECAUSE WE PLACED THE FOOD IN COOKING POT
            m_Food = null;
        }
    }

    private void OnFoodPick(Food food) {
        if (HasFoodInHands()) {
            Debug.LogError("Player already has a food in hand. Place the food in cooking pot to pick another");
            return;
        }
        m_Food = food;
    }

    public bool HasFoodInHands() {
        return m_Food != null;
    }

    public Food GetCurrentFood() {
        if (!HasFoodInHands()) {
            return null;
        }
        return m_Food;
    }
}
