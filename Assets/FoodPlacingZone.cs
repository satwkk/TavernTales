using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodPlacingZone : InteractableBase, IFoodPlacer
{
    public Transform m_FoodPlacingTransform;

    public override void Interact(IInteractionActor interactingActor)
    {
        if (interactingActor is not IFoodOrderService foodService)
            return;
        
        foodService.PlaceFood(this);
    }

    public Vector3 GetFoodPlacingLocation()
    {
        return m_FoodPlacingTransform.position;
    }
}
