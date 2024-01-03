using System;
using UnityEngine;

namespace LHC.Customer.StateMachine
{
    public interface IState
    {
        void OnEnter();
        void OnTick();
        void OnExit();
        void SwitchState(IState newState);
    }
}