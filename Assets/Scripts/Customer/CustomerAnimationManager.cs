using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CustomerAnimationManager : MonoBehaviour
{
    [SerializeField] private Animator m_Animator;
    public Animator Animator => m_Animator;

    private void Start()
    {
    }

    public void PlayWalkingAnimation(bool value)
    {
        m_Animator.SetBool("isWalking", value);
    }
}
