using UnityEngine;
using System;

public interface IFoodOrderService
{
    public Transform FoodHolder { get; set; }
    public Food CurrentPickupFood { get; set; }
    void PickupFood(Food food);
    void PlaceFood(IFoodPlacer foodPlacer);
    Food GetFoodInHands();
}
