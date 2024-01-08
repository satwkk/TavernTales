using UnityEngine;

namespace LHC.Tavern
{
    using System;
    using System.Collections;
    using LHC.Customer;
    using LHC.Globals;
    
    public class Seat : MonoBehaviour
    {
        public Customer SeatingCustomer { get; private set; }
        public Transform SeatingCustomerTransform { get; private set; }
        [field: SerializeField] public Transform SeatingOffsetTransform { get; set; }

        private void Awake()
        {
            SeatingOffsetTransform = transform.Find("SKT_Sitting_Location");
        }

        private void Update()
        {
        }

        public void Sit(Customer customer, Action after = null) 
        {
            SeatingCustomer = customer;
            SeatingCustomerTransform = SeatingCustomer.transform;
            StartCoroutine(SitCoroutine(after));
        }

        private IEnumerator SitCoroutine(Action after = null)
        {
            var forwardDir = this.transform.forward;

            // TURN TOWARDS THE TABLE
            while (SeatingCustomerTransform.forward != forwardDir)
            {
                // SeatingCustomerTransform.forward = Vector3.MoveTowards(SeatingCustomerTransform.forward, forwardDir, 2f * Time.deltaTime);
                SeatingCustomerTransform.forward = Vector3.Lerp(SeatingCustomerTransform.forward, forwardDir, 3f * Time.deltaTime);
                yield return null;
            }

            // PLAY THE SITTING ANIMATION
            SeatingCustomer.GetAnimationManager().PlaySittingAnimation(true);

            // MOVE THE COLLIDER DOWN TO THE OFFSET TO ALIGN THE CUSTOMER WITH THE CHAIR
            while (Vector3.SqrMagnitude(SeatingCustomerTransform.position - SeatingOffsetTransform.position) > 0.2f * 0.2f)
            {
                SeatingCustomerTransform.position = Vector3.MoveTowards(SeatingCustomerTransform.position, SeatingOffsetTransform.position, 1f * Time.deltaTime);
                yield return null;
            }
        }
    }
}