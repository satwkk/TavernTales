using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Utility
{
    public class CaptureFoodIcon : MonoBehaviour
    {
        
    }
    
    #if UNITY_EDITOR
    
    public class CaptureFoodIconEditor : Editor
    {
        private List<GameObject> foodObjects;
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Capture"))
            {
                foreach (var obj in foodObjects)
                {
                }
            }
        }
    }
    
    #endif
}