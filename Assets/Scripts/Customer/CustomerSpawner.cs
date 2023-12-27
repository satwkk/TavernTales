using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour {
    public GameObject m_CustomerPrefab;
    public List<Transform> m_SpawnPoints;
    public int m_SpawnAmount;
}

[CustomEditor(typeof(CustomerSpawner))]
public class CustomerSpawnerEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        var customerSpawner = (CustomerSpawner)target;
        if (GUILayout.Button("Spawn Character")) {
            for (int i = 0; i < customerSpawner.m_SpawnAmount; i++) {
                var spawnPos = customerSpawner.m_SpawnPoints[UnityEngine.Random.Range(0, customerSpawner.m_SpawnPoints.Count)];
                Instantiate(customerSpawner.m_CustomerPrefab, spawnPos.position, Quaternion.identity);
            }
        }
    }
}
