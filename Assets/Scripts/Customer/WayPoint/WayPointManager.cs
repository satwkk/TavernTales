using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointManager : MonoBehaviour
{
    public List<WayPoint> approachShopWayPoint;
    public List<WayPoint> leaveShopWayPoint;
    public List<WayPoint> orderFoodWayPoint_1;
    public List<WayPoint> orderFoodWayPoint_2;

    public static WayPointManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
    }

    public List<WayPoint> GetOrderFoodWayPoint()
    {
        int random = UnityEngine.Random.Range(0, 1);
        if (random == 0) return orderFoodWayPoint_1;
        else return orderFoodWayPoint_2;
    }
}
