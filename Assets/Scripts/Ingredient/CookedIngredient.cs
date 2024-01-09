using UnityEngine;

public class CookedIngredient : Food
{
    public Ingredient OriginalIngredient;

    public override void Interact(IInteractionActor interactingActor)
    {
        base.Interact(interactingActor);
    }
}
