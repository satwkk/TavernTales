using UnityEngine;
using UnityEngine.UI;

namespace UI.Customer
{
    public class FoodOrderUI : MonoBehaviour
    {
        public Image foodImageIcon;
        public Slider customerWaitDurationSlider;

        public void CraftUI(Ingredient ingredientData)
        {
            foodImageIcon.sprite = ingredientData.IngredientData.FoodIcon.sprite;
        }

        public void CleanUp()
        {
            foodImageIcon.sprite = null;
            // customerWaitDurationSlider.value = 0;
        }

        public void Show() => gameObject.SetActive(true);
        
        public void Hide()
        {
            CleanUp();
            gameObject.SetActive(false);
        }

    }
}