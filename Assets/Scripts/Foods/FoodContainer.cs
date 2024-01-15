using UnityEngine;

public class FoodPlate : InteractableBase
{
    public GameObject FoodPrefab;
    public Food Food;
    public float Quantity = 10;

    public override void Interact(IInteractionActor interactingActor)
    {
        if (Quantity > 0)
        {
            var foodPrefab = Instantiate(FoodPrefab, transform.position, Quaternion.identity);
            Quantity -= 1;
            Food = foodPrefab.GetComponent<Food>();
            Food.Interact(interactingActor);
        }
        else 
        {
            Debug.LogError("There are no foods in the container, get some");
        }
    }

}
