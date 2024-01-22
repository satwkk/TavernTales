using System;
using LHC.Globals;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Bird
{
    public class Bird : MonoBehaviour
    {
        public Rigidbody rigidBody;
        public Collider flyingAreaCollider;
        public Bounds flyingBounds;
        public Vector3 randomPoint;
        public BirdData birdData;

        private void Awake()
        {
            rigidBody = GetComponent<Rigidbody>();
            flyingAreaCollider = GameObject.Find(Constants.BIRDS_FLYING_ZONE).GetComponent<BoxCollider>();
            flyingBounds = flyingAreaCollider.bounds;
            randomPoint = FindRandomPoint(flyingBounds);
        }

        private void FixedUpdate()
        {
            if (Vector3.SqrMagnitude(randomPoint - transform.position) < 2f * 2f)
            {
                Debug.LogError("About to reach the random point, find another");
                randomPoint = FindRandomPoint(flyingBounds);
            }
            MoveTowards();
            RotateTowards();
        }

        private void MoveTowards()
        {
            var dir = transform.forward;
            rigidBody.velocity = dir * (birdData.moveSpeed * Time.fixedDeltaTime);
            rigidBody.velocity += transform.up * (Mathf.Sin(Time.time * birdData.amplitude) * birdData.frequency);
        }

        private void RotateTowards()
        {
            var dirToTarget = (randomPoint - transform.position).normalized;
            transform.forward = Vector3.Lerp(transform.forward, dirToTarget, birdData.turnSpeed * Time.fixedDeltaTime);
        }

        // Returns a random point in a box collider bound
        private Vector3 FindRandomPoint(Bounds flyingBound)
        {
            var randomX = Random.Range(flyingBounds.min.x, flyingBounds.max.x);
            // var randomY = Random.Range(flyingBounds.min.y, flyingBounds.max.y);
            var randomZ = Random.Range(flyingBounds.min.z, flyingBounds.max.z);
            return new Vector3(randomX, transform.position.y, randomZ);
        }

        // Returns a random point in bird's forward direction offseted to a random value on X axis
        private Vector3 FindRandomPoint()
        {
            var pos = transform.position + transform.forward * birdData.lookAhead;
            pos.x = Random.Range(-90, 90);
            pos.y = Random.Range(-1, 1);
            return pos;
        }

        private void OnDrawGizmos()
        {
            if (randomPoint != Vector3.zero)
            {
                Handles.DrawWireCube(randomPoint, Vector3.one * .2f);
            }
        }
    }
}