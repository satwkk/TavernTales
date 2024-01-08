using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LHC.Tavern
{
    public class EatingTable : MonoBehaviour
    {
        [SerializeField] private List<Seat> unoccupiedSeats;
        [SerializeField] private List<Seat> occupiedSeats;

        public bool IsFull => unoccupiedSeats.Count == 0;

        private void Awake() 
        {
            unoccupiedSeats = GetComponentsInChildren<Seat>().ToList();
        }

        public Seat GetSeatForCustomer()
        {
            var randomSeat = unoccupiedSeats[UnityEngine.Random.Range(0, unoccupiedSeats.Count)];
            unoccupiedSeats.Remove(randomSeat);
            occupiedSeats.Add(randomSeat);
            return randomSeat;
        }
    }
}