using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LHC.Globals;

public class CookingPot : InteractableBase, IFoodPlacer {

    // THE COOKING PARTICLE SYSTEM THAT IS TRIGGERED WHEN THE COOK IS BEING PREPARED
    [field: SerializeField] public ParticleSystem CookingParticle;

    // LIST OF FOODS THAT ARE BEING ADDED TO THE POT
    [SerializeField] private List<Food> foods;

    public Transform LerpToSocketAfterCurrentIngredientCooked;

    public Ingredient CurrentIngredient;
    
    public CookedIngredient CurrentCookedIngredient;

    public GameObject CurrentCookedIngredientObj;

    public static CookingPot Instance { get; private set; }
    
    public static event Action<Food> OnFoodAddToPotEvent;

    private void Awake() 
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        LerpToSocketAfterCurrentIngredientCooked = transform.Find(Constants.COOKINGPOT_COOKEDFOOD_SOCKET);
    }

    public void CreateCookedIngredient(Ingredient ingredient)
    {
        CurrentIngredient = ingredient;
        CurrentCookedIngredientObj = Instantiate(CurrentIngredient.IngredientData.CookedIngredientPrefab, transform.position, Quaternion.identity);
        CurrentCookedIngredient = CurrentCookedIngredientObj.GetComponent<CookedIngredient>();
        CurrentCookedIngredient.OriginalIngredient = CurrentIngredient;
        StartCoroutine(StartCookFinishAnimation());
    }

    private IEnumerator StartCookFinishAnimation()
    {
        while (CurrentCookedIngredient.transform.position != LerpToSocketAfterCurrentIngredientCooked.position)
        {
            CurrentCookedIngredient.transform.position = Vector3.MoveTowards(
                CurrentCookedIngredient.transform.position, 
                LerpToSocketAfterCurrentIngredientCooked.position, 
                2f * Time.deltaTime
            );
            yield return null;
        }
    }

    public void PlayCookingParticle()
    {
        if (CookingParticle.isPlaying)
            return;
        CookingParticle.Play();
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
        foods.Add(foodInHands);

        // Fire an event to disable the game object
        OnFoodAddToPotEvent?.Invoke(foodInHands);
    }

    public Vector3 GetFoodPlacingLocation()
    {
        return transform.position;
    }
}
