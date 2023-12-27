using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngredientUI : MonoBehaviour {
    public TextMeshProUGUI ingredientNameUI;
    public TextMeshProUGUI ingredientDurationUI;
    public Slider remainingDuration;
    public CurrentIngredientData m_CurrentIngredientData;

    private void OnEnable() {
        CookingPot.OnIngredientCreate_Event += OnIngredientCreateCallback;
        CookingPot.OnIngredientCookFail_Event += OnIngredientDurationFinishCallback;
        CookingPot.OnIngredientCookSuccess_Event += OnIngredientDurationFinishCallback;
        CurrentIngredientData.OnPrepationDurationValueChange_Event += OnPrepationDurationValueChangeCallback;
    }

    private void OnDisable() {
        CookingPot.OnIngredientCreate_Event -= OnIngredientCreateCallback;
        CookingPot.OnIngredientCookFail_Event -= OnIngredientDurationFinishCallback;
        CookingPot.OnIngredientCookSuccess_Event -= OnIngredientDurationFinishCallback;
        CurrentIngredientData.OnPrepationDurationValueChange_Event -= OnPrepationDurationValueChangeCallback;
    }

    private void OnIngredientDurationFinishCallback() {
        ingredientNameUI.text = "";
        ingredientDurationUI.text = 0.ToString();
        remainingDuration.value = 1f;
    }

    private void OnPrepationDurationValueChangeCallback(float newDuration) {
        ingredientDurationUI.text = newDuration.ToString();
        remainingDuration.value = newDuration / m_CurrentIngredientData.currentIngredient.IngredientData.prepareDuration;
    }

    public void OnIngredientCreateCallback(CurrentIngredientData currentIngredientData) {
        m_CurrentIngredientData = currentIngredientData;
        ingredientNameUI.text = m_CurrentIngredientData.currentIngredient.IngredientData.name;
        ingredientDurationUI.text = m_CurrentIngredientData.currentIngredient.IngredientData.prepareDuration.ToString();
        remainingDuration.value = 1f;
    }
}
