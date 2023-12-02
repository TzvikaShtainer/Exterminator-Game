using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Prefabs.Framework.AI.Perception
{
    public class PerceptionComponent : MonoBehaviour
    {
        [SerializeField] private SenseComponent[] senses;
        LinkedList<PerceptionStimuli> currentlyPerceivedStimulis = new LinkedList<PerceptionStimuli>();

        PerceptionStimuli targetStimuli;

        public delegate void OnPerceptionTargetChanged(GameObject target, bool sensed);
        public event OnPerceptionTargetChanged onPerceptionTargetChanged;

        [Header("Audio")]
        [SerializeField] private AudioClip perceptionAudio;
        [SerializeField] private float volume = 1;
        

        private void Awake()
        {
            foreach (SenseComponent sense in senses)
            {
                sense.onPerceptionUpdated += SenseUpdated;
            }
        }

        private void OnDisable()
        {
            foreach (SenseComponent sense in senses)
            {
                sense.onPerceptionUpdated -= SenseUpdated;
            }
        }

        private void SenseUpdated(PerceptionStimuli stimuli, bool successfullySensed)
        {
            var nodeFound =  currentlyPerceivedStimulis.Find(stimuli);
        
            if (successfullySensed)
            {
                if (nodeFound != null)
                {
                    currentlyPerceivedStimulis.AddAfter(nodeFound, stimuli);
                }
                else
                {
                    currentlyPerceivedStimulis.AddLast(stimuli);
                }
            }
            else
            {
                currentlyPerceivedStimulis.Remove(nodeFound);
            }

            if (currentlyPerceivedStimulis.Count != 0)
            {
                PerceptionStimuli highestStimuli = currentlyPerceivedStimulis.First.Value;
                if (targetStimuli == null || targetStimuli != highestStimuli)
                {
                    targetStimuli = highestStimuli;
                    onPerceptionTargetChanged?.Invoke(targetStimuli.gameObject, true);

                    Vector3 audioPos = transform.position;
                    GamePlayStatics.PlayAudioAtLoc(perceptionAudio, audioPos, volume);
                }
            }
            else
            {
                if (targetStimuli != null)
                {
                    onPerceptionTargetChanged?.Invoke(targetStimuli.gameObject, false);
                    targetStimuli = null;
                }
            }
        }

        internal void AssignPerceivedStimuli(PerceptionStimuli targetStimuli)
        {
            if (senses.Length != 0)
            {
                senses[0].AssignPerceivedStimuli(targetStimuli);
            }
        }
    }
}
