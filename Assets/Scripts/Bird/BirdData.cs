using UnityEngine;

namespace Bird
{
    [CreateAssetMenu(fileName = "Bird", menuName = "Bird/Bird Data")]
    public class BirdData : ScriptableObject
    {
        public float moveSpeed;
        public float turnSpeed;
        public float amplitude;
        public float frequency;
        public float lookAhead;
    }
}