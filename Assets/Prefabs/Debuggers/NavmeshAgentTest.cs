using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavmeshAgentTest : MonoBehaviour
{
    NavMeshAgent navAgent;
    [SerializeField] Transform targetPos;

    private void Awake() {
        navAgent = GetComponent<NavMeshAgent>();
    }

    

    void Start()
    {
        
    }

    void Update()
    {
        navAgent.SetDestination(targetPos.transform.position);
    }
}
