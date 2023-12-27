using System;
using System.Collections;
using UnityEngine;

// [System.Serializable]
// public struct CurrentIngredientData {
//     public Ingredient currentIngredient;
//     public float prepationDurationLeft;
// }

public class IngredientManager : MonoBehaviour {

    public CurrentIngredientData m_CurrentIngredientData;
    public CookingPot m_CookingPot;
    public static Action<Ingredient> OnIngredientCreate;
    public static Action<CurrentIngredientData> OnIngredientPreparationValueChange;
    public static Action<CurrentIngredientData> OnIngredientDurationFinish;

    private void OnEnable() {
        IngredientSpawner.OnIngredientCreate_Event += OnIngredientCreateCallback;
        CookingPot.OnIngredientCookSuccess_Event += FinishIngredientPrepationDurationCallback;
        CookingPot.OnIngredientCookFail_Event += FinishIngredientPrepationDurationCallback;
    }

    private void OnDisable() {
        IngredientSpawner.OnIngredientCreate_Event -= OnIngredientCreateCallback;
        CookingPot.OnIngredientCookSuccess_Event -= FinishIngredientPrepationDurationCallback;
        CookingPot.OnIngredientCookFail_Event -= FinishIngredientPrepationDurationCallback;
    }


    public void FinishIngredientPrepationDurationCallback() {
        StopAllCoroutines();
        m_CurrentIngredientData.currentIngredient = null;
        m_CurrentIngredientData.prepationDurationLeft = 0f;
    }

    public void OnIngredientCreateCallback(Ingredient ingredient) {
        // TODO: Do we really need another event here, maybe we can perform dependency injection in some sort of way
        OnIngredientCreate?.Invoke(ingredient);
        m_CurrentIngredientData.currentIngredient = ingredient;
        m_CurrentIngredientData.prepationDurationLeft = ingredient.IngredientData.prepareDuration;
        StartCoroutine(StartIngredientPreparationTimer());
    }

    IEnumerator StartIngredientPreparationTimer() {
        while (m_CurrentIngredientData.prepationDurationLeft >= 0) {
            if (m_CurrentIngredientData.prepationDurationLeft <= 0) {
                Debug.Log("Preparation time completed");
                m_CurrentIngredientData.currentIngredient = null;
                m_CurrentIngredientData.prepationDurationLeft = 0f;
                OnIngredientDurationFinish?.Invoke(m_CurrentIngredientData);
                yield return null;
            }
            Debug.Log(m_CurrentIngredientData.prepationDurationLeft);
            m_CurrentIngredientData.prepationDurationLeft -= 1f;
            OnIngredientPreparationValueChange?.Invoke(m_CurrentIngredientData);
            yield return new WaitForSeconds(1f);
        }
    }
}
