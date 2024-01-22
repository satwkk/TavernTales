using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookedIngredientServingZone : InteractableBase, IFoodPlacer
{
    public Transform PlacingOffsetTransform;
    public CookedIngredient CookedIngredient;
    public static event Action<CookedIngredient> OnCookedIngredientPlaced;
    public static Action OnFoodPickupByCustomer;

    private void Start()
    {
        if (PlacingOffsetTransform == null)
        {
            PlacingOffsetTransform = transform;
        }
    }

    public override void Interact(IInteractionActor interactingActor)
    {
        if (interactingActor is not IFoodOrderService orderService)
        {
            Debug.LogError("Entity interacting with this is not a IFoodOrderService");
            return;
        }

        var cookedIngredient = (CookedIngredient)orderService.GetFoodInHands();
        if (cookedIngredient is null)
        {
            Debug.LogError("No Cooked ingredient in hands");
            return;
        }
        
        CookedIngredient = cookedIngredient;
        orderService.PlaceFood(this);
        CookedIngredient.OriginalIngredient.IsServed = true;
        OnCookedIngredientPlaced?.Invoke(CookedIngredient);
    }

    public Vector3 GetFoodPlacingLocation()
    {
        return PlacingOffsetTransform.position;
    }

    private void CleanUp()
    {
        
    }
}

