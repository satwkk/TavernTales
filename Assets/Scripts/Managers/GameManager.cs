using System;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable() {
        //IngredientSpawnerEditor.OnIngredientCreate += OnIngredientCreate;
    }

    private void OnDisable() {
        //IngredientSpawnerEditor.OnIngredientCreate -= OnIngredientCreate;
    }

    private void OnIngredientCreate(Ingredient ingredient) {
        Debug.LogWarning("INGREDIENT CREATED");
        Debug.Log("Name: " + ingredient.IngredientData.name);
    }
}
