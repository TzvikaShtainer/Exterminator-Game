using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prefabs.Framework.AI.Perception
{
    public abstract class SenseComponent : MonoBehaviour
    {
        [SerializeField] private float forgettingTime = 3f;
        static List<PerceptionStimuli> registeredStimulis = new List<PerceptionStimuli>(); 
        List<PerceptionStimuli> perceivableStimulis = new List<PerceptionStimuli>();

        private Dictionary<PerceptionStimuli, Coroutine> forgettingRoutines = new Dictionary<PerceptionStimuli, Coroutine>();

        public delegate void OnPerceptionUpdated(PerceptionStimuli stimuli, bool successfullySensed);
        public event OnPerceptionUpdated onPerceptionUpdated;

        protected abstract bool IsStimuliSensable(PerceptionStimuli stimuli);
        public static void RegisterStimuli(PerceptionStimuli stimuli)
        {
            if (registeredStimulis.Contains(stimuli))
                return;
            
            registeredStimulis.Add(stimuli);
        }

        public static void UnRegisterStimuli(PerceptionStimuli stimuli)
        {
            registeredStimulis.Remove(stimuli);
        }
        
        private void Update()
        {
            foreach (var stimuli in registeredStimulis)
            {
                if (IsStimuliSensable(stimuli))
                {
                    if (!perceivableStimulis.Contains(stimuli))
                    {
                        perceivableStimulis.Add(stimuli);
                        
                        if (forgettingRoutines.TryGetValue(stimuli, out Coroutine coroutine)) //if we see the stimuli but we are still in the forget process
                        {
                            StopCoroutine(coroutine);
                            forgettingRoutines.Remove(stimuli);
                        }
                        else
                        {
                            onPerceptionUpdated?.Invoke(stimuli, true);
                            Debug.Log($"i just sense {stimuli.gameObject}");
                        }
                    }
                }
                else
                {
                    if (perceivableStimulis.Contains(stimuli))
                    {
                        perceivableStimulis.Remove(stimuli);
                        forgettingRoutines.Add(stimuli, StartCoroutine(ForgetStimuli(stimuli)));
                    }
                }
            }
        }

        IEnumerator ForgetStimuli(PerceptionStimuli stimuli)
        {
            yield return new WaitForSeconds(forgettingTime);
            forgettingRoutines.Remove(stimuli);
            onPerceptionUpdated?.Invoke(stimuli, false);
            Debug.Log($"i lost track of {stimuli.gameObject}");
        }
        protected virtual void DrawDebug()
        {
            
        }
        private void OnDrawGizmos()
        {
            DrawDebug();
        }

        public void AssignPerceivedStimuli(PerceptionStimuli targetStimuli)
        {
            perceivableStimulis.Add(targetStimuli);
            onPerceptionUpdated?.Invoke(targetStimuli, true);
            
            //TODO what if we are forgetting it
            if (forgettingRoutines.TryGetValue(targetStimuli, out Coroutine forgetCoroutine))
            {
                StopCoroutine(forgetCoroutine);
                forgettingRoutines.Remove(targetStimuli);
            }
        }
    }
}
