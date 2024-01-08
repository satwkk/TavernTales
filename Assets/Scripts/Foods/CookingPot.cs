using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CookingPot : InteractableBase, IFoodPlacer {

    [SerializeField] private List<Food> foods;
    
    public static event Action<Food> OnFoodAddToPotEvent;

    public override void Interact(IInteractionActor interactingActor)
    {
        if (interactingActor is not IFoodOrderService foodService)
        {
            Debug.LogError( "The interacting actor is not an IFoodOrderService" );
            return;
        }

        Interact( foodService  );
    }

    private void Interact(IFoodOrderService foodCharacter) 
    {
        // Get the current food from player's hands
        var foodInHands = foodCharacter.GetFoodInHands();
        foodCharacter.PlaceFood(this);

        // Add the food to the list
        foods.Add(foodInHands);

        // Fire an event to disable the game object
        OnFoodAddToPotEvent?.Invoke(foodInHands);
    }

    public Vector3 GetFoodPlacingLocation()
    {
        return transform.position;
    }
}
