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
        private MonoBehaviour m_MonoActor;
        private bool m_CanRun;

        public Timer( MonoBehaviour mono, float threshold, Action onComplete )
        {
            m_MonoActor = mono;
            m_Threshold = threshold;
            OnTimerComplete = onComplete;
            m_CanRun = true;
            m_MonoActor.StartCoroutine( StartTimer() );
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
                if (!m_CanRun)
                {
                    UnityEngine.Debug.Log("Timer stopped in between");
                    yield break;
                }
                OnTimerTicking?.Invoke();
                currentTimer += 1f;
                yield return new WaitForSeconds( 1f );
            }
            OnTimerComplete?.Invoke();
            yield return null;
        }

        public void StopTimer()
        {
            m_CanRun = false;
        }
    }
}
