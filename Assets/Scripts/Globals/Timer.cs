using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace LHC.Globals
{

    class Timer
    {
        private float m_Threshold;
        private Action OnTimerTicking;
        private Action OnTimerComplete;

        public Timer( MonoBehaviour mono, float threshold, Action onComplete )
        {
            m_Threshold = threshold;
            OnTimerComplete = onComplete;
            mono.StartCoroutine( StartTimer() );
        }

        public Timer ( MonoBehaviour mono, float threshold, Action onTicking, Action onComplete )
        {
            m_Threshold = threshold;
            OnTimerTicking = onTicking;
            OnTimerComplete = onComplete;
            mono.StartCoroutine( StartTimer() );
        }

        public IEnumerator StartTimer()
        {
            var currentTimer = 0f;
            while (currentTimer <= m_Threshold)
            {
                OnTimerTicking?.Invoke();
                currentTimer += 1f;
                yield return new WaitForSeconds( 1f );
            }
            OnTimerComplete?.Invoke();
        }
    }
}
