using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestingField : InteractableBase
{
    public GameObject harvestingFoodPrefab;
    public GameObject currentFoodSpawnObj;
    public bool isInteracted = false;
    public float interactionHoldDuration = 4;
    public Vector3 interactingActorLocation;
    public float currentInteractionDuration;
    public Transform spawnLocation;

    private void Awake() 
    {
        spawnLocation = transform.parent.Find("Spawn Location");
    }

    public override void Interact(IInteractionActor interactingActor)
    {
        isInteracted = true;
        var loc = interactingActor.GetInteractionPosition();
        interactingActorLocation = new Vector3(loc.x, 0f, loc.y);
    }

    private void Update()
    {
        if (!isInteracted)  
            return;

        if (Input.GetKey(KeyCode.E))
        {
            if (currentInteractionDuration >= interactionHoldDuration)
            {
                Debug.LogError("Spawning foods");
                currentInteractionDuration = 0;
                isInteracted = false;
                var randomSpawnThreshold = UnityEngine.Random.Range(5, 10);
                for (int i = 0; i < randomSpawnThreshold; i++) 
                {
                    currentFoodSpawnObj = Instantiate(
                        harvestingFoodPrefab, 
                        spawnLocation.position + Random.insideUnitSphere,
                        Quaternion.identity
                    );
                    //currentFoodSpawnObj.GetComponent<Food>().OnSpawn();
                }
            }
            currentInteractionDuration += Time.deltaTime;
        }
    }
}
