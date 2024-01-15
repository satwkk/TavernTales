using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngredientUI : MonoBehaviour {
    public TextMeshProUGUI ingredientNameUI;
    public Ingredient m_CurrentIngredientData;

    private void OnEnable() {
        IngredientSpawner.OnIngredientCreate_Event += OnIngredientCreateCallback;
    }

    private void OnDisable() {
        IngredientSpawner.OnIngredientCreate_Event -= OnIngredientCreateCallback;
    }

    public void OnIngredientCreateCallback(Ingredient currentIngredientData) {
        m_CurrentIngredientData = currentIngredientData;
        ingredientNameUI.text = m_CurrentIngredientData.IngredientData.Name;
    }
}
