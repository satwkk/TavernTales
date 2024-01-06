using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingPot : InteractableBase, IFoodPlacer {

    public List<Food> m_Foods;
    public static event Action<Food> OnFoodAddToPotEvent;

    private void Start() 
    {
        m_Foods = new List<Food>();
    }

    private void OnEnable() 
    {
    }
    
    private void OnDisable() 
    {
    }

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
        m_Foods.Add(foodInHands);

        // Fire an event to disable the game object
        OnFoodAddToPotEvent?.Invoke(foodInHands);
    }

    public Vector3 GetFoodPlacingLocation()
    {
        return transform.position;
    }
}
