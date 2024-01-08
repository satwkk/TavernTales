using System.Collections.Generic;
using System.Linq;
using LHC.Customer;
using UnityEngine;

namespace LHC.Tavern
{
    public class Tavern : MonoBehaviour
    {
        public static Tavern Instance { get; private set; }
        [field: SerializeField] public EatingTable[] EatingTables { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            
            EatingTables = GetComponentsInChildren<EatingTable>();
        }

        public EatingTable GetAvailableEatingTable()
        {
            var availableEatingTables = EatingTables.Where(x => x.IsFull == false).ToArray();
            return availableEatingTables[UnityEngine.Random.Range(0, availableEatingTables.Length-1)];
        }
    }
}

