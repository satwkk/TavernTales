using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LHC.Globals;
using System;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class CustomerAnimationManager : MonoBehaviour
{
    [field: SerializeField] public Animator Animator { get; private set; }

    public Action OnPickupAnimationFinish;
    public Action OnPickupAttachDurationReached;

    [field: Header("Layer Weights")] [field: Space()]
    [field: SerializeField] public float CurrentPickupLayerWeight { get; set; } = 0;
    [field: SerializeField] public float CurrentDrinkingLayerWeight { get; set; } = 0;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
    }

    public void PlayWalkingAnimation(bool value)
    {
        Animator.SetBool("isWalking", value);
    }

    public void PlayPickupAnimation(bool value, Action after = null) 
    {
        // SET THE PICKUP ANIMATION LAYER WEIGHT TO 1
        Animator.SetLayerWeight(Constants.PICKUP_ANIMATION_LAYER, 1f);

        // SET THE PICKUP TRIGGER
        Animator.SetBool(Constants.PICKUP_ANIMATION_TRIGGER_CONDITION, value);

        // ADD CALLBACK ON WHAT HAPPENS AFTER PICKUP ANIMATION IS FINISHED
        OnPickupAnimationFinish += after;
    }

    // CALLED FROM ANIMATION EVENT CREATED IN PICKUP ANIMATION
    public void AttachFoodWhilePlayingPickupAnimation()
    {
        Debug.LogWarning("AttachFoodWhilePlayingPickupAnimation called");
        OnPickupAttachDurationReached?.Invoke();
    }

    public void PlayEatingAnimation(Action after = null) 
    {
        Animator.SetLayerWeight(Constants.EATING_ANIMATION_LAYER, 1f);
    }
    
    public void PlaySittingAnimation(bool value)
    {
        Animator.SetBool(Constants.SITTING_ANIMATION_TRIGGER_CONDITION, value);
    }
}
