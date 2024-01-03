using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CurrentIngredientData {
    public Ingredient currentIngredient;
    public static Action<float> OnPrepationDurationValueChange_Event;
    public float prepationDurationLeft;
    public float PrepationDurationLeft {
        get {
            return prepationDurationLeft;
        }
        set {
            prepationDurationLeft = value;
            OnPrepationDurationValueChange_Event?.Invoke(prepationDurationLeft);
        }
    }
    public int foodsLeftForIngredient;
}

public class CookingPot : InteractableBase {

    public List<Food> m_Foods;

    [Header("DEBUG ONLY DO NOT CHANGE THIS SETTINGS MANUALLY")]
    [Space()]
    public CurrentIngredientData m_CurrentIngredientData;

    // ============================ EVENTS =============================
    public static Action<CurrentIngredientData> OnIngredientCreate_Event;
    public static Action<Food> OnFoodAddToPot_Event;
    public static Action OnIngredientCookSuccess_Event;
    public static Action OnIngredientCookFail_Event;

    void Start() {
        m_Foods = new List<Food>();
    }

    private void OnEnable() {
        IngredientSpawner.OnIngredientCreate_Event += OnIngredientCreateCallback;
    }
    
    private void OnDisable() {
        IngredientSpawner.OnIngredientCreate_Event -= OnIngredientCreateCallback;
    }

    private bool HasIngredientToCook() {
        return m_CurrentIngredientData.currentIngredient != null;
    }

    private void SetFoodLeftCount(int newCount) {
        m_CurrentIngredientData.foodsLeftForIngredient = newCount;
    }

    private void ClearVariables() {
        m_CurrentIngredientData.currentIngredient = null;
        m_CurrentIngredientData.foodsLeftForIngredient = 0;
        m_Foods.Clear();
    }

    /// <summary>
    /// Callback called when an ingredient is created and spawned by the ingredient spawner.
    /// </summary>
    /// <param name="ingredient"> The ingredient that was created </param>
    public void OnIngredientCreateCallback(Ingredient ingredient) {
        StopAllCoroutines();
        m_CurrentIngredientData.currentIngredient = ingredient;
        m_CurrentIngredientData.foodsLeftForIngredient = ingredient.IngredientData.requiredFoods.Count;
        m_CurrentIngredientData.PrepationDurationLeft = ingredient.IngredientData.prepareDuration;
        OnIngredientCreate_Event?.Invoke(m_CurrentIngredientData);
        StartCoroutine(StartIngredientPreparationTimer());
    }

    IEnumerator StartIngredientPreparationTimer() {
        Debug.LogWarning("Hello from coroutine");
        while (m_CurrentIngredientData.PrepationDurationLeft > 0) {
            if (m_CurrentIngredientData.foodsLeftForIngredient == 0) {
                Debug.Log("wait someone cooked here");
                ClearVariables();
                OnIngredientCookSuccess_Event?.Invoke();
                yield break;
            }
            m_CurrentIngredientData.PrepationDurationLeft -= 1;
            yield return new WaitForSeconds(1f);
        }

        // Prepation duration has passed and the value is <= 0
        Debug.Log("he could not cook");
        ClearVariables();
        OnIngredientCookFail_Event?.Invoke();
    }

    public override void Interact( IInteractionActor interactingActor )
    {
        var foodService = interactingActor as IFoodOrderService;
        if (foodService == null)
        {
            Debug.Log( "The interacting actor is not an IFoodOrderService" );
            return;
        }

        Interact( interactingActor  );
    }

    public void Interact(IFoodOrderService foodCharacter) {
        if (!HasIngredientToCook()) {
            Debug.LogError("You have not accepted any order to cook");
            return;
        }

        // Get the current food from player's hands
        Food currFood = foodCharacter.GetFoodInHands();
        if (currFood == null) {
            Debug.LogError("Player has no food in hands to add to pot");
            return;
        }

        // Add the food to the list
        m_Foods.Add(currFood);

        // Check if the added food is present in current ingredient's required foods
        // If yes decrement the required food count by 1
        if (
            m_CurrentIngredientData.currentIngredient != null && 
            m_CurrentIngredientData.currentIngredient.IngredientData.HasFoodInRequiredItems(currFood)
            ) {
            this.SetFoodLeftCount(m_CurrentIngredientData.foodsLeftForIngredient - 1);
        }

        // Fire an event to disable the game object
        // TODO: Disable the collider too and move the gameobject to the pot's location
        OnFoodAddToPot_Event?.Invoke(currFood);
    }
}
