using System;
using System.Collections;
using LHC.Globals;
using UnityEngine;

namespace LHC.Customer.StateMachine
{
    public class AngryState : BaseState
    {

        public AngryState(Customer controller, CustomerData customerData) : base(controller, customerData)
        {
        }

        public override void OnEnter()
        {
            m_Customer.StartCoroutine(LeaveShopCoroutine());
        }

        private IEnumerator LeaveShopCoroutine()
        {
            m_Customer.AnimationManager.Animator.SetTrigger(Constants.ANGRY_ANIMATION_TRIGGER_CONDITION);
            yield return new WaitForSeconds(1.6f);
            Debug.LogError("now going to leaving state");
            SwitchState(m_Customer.LeaveState);
        }

        public override void OnExit()
        {
        }

        public override void OnTick()
        {
        }
    }
}