using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFoodOrderService 
{
    public Action<Food> OnPickFood_Event { get; set; }

    void PickupFood( Food food );
    Food GetFoodInHands();
}
