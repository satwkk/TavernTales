using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodPlate : InteractableBase
{
    public GameObject m_FoodPrefab;
    public Food m_Food;

    public override void Interact(IInteractionActor interactingActor)
    {
        var foodPrefab = Instantiate(m_FoodPrefab, transform.position, Quaternion.identity);
        m_Food = foodPrefab.GetComponent<Food>();
        m_Food.Interact(interactingActor);
    }

}
