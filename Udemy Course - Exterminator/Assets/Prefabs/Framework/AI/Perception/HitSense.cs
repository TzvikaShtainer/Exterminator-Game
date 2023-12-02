using System;
using System.Collections;
using System.Collections.Generic;
using Prefabs.Framework.Health;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Prefabs.Framework.AI.Perception
{
    public class HitSense : SenseComponent
    {
        [FormerlySerializedAs("healthComp")] [SerializeField] private HealthComponent healthComponent;
        [SerializeField] private float hitMemory = 2f;

        private Dictionary<PerceptionStimuli, Coroutine> hitsRecord = new Dictionary<PerceptionStimuli, Coroutine>();
        protected override bool IsStimuliSensable(PerceptionStimuli stimuli)
        {
            return hitsRecord.ContainsKey(stimuli);
        }

        private void OnDisable()
        {
            healthComponent.onTakeDamage -= TookDamage;
        }

        private void Start()
        {
            healthComponent.onTakeDamage += TookDamage;
        }
        
        private void TookDamage(float health, float amt, float maxHealth, GameObject instigator)
        {
            PerceptionStimuli stimuli = instigator.GetComponent<PerceptionStimuli>();
            if (stimuli != null)
            {
                Coroutine newForgettingCoroutine = StartCoroutine(ForgetStimuli(stimuli));
                if (hitsRecord.TryGetValue(stimuli, out Coroutine onGoingCoroutine))
                {
                    StopCoroutine(onGoingCoroutine);
                    hitsRecord[stimuli] = newForgettingCoroutine;
                }
                else
                {
                    hitsRecord.Add(stimuli, newForgettingCoroutine);
                }
            }
        }
        
        IEnumerator ForgetStimuli(PerceptionStimuli stimuli)
        {
            yield return new WaitForSeconds(hitMemory);
            hitsRecord.Remove(stimuli);
        }
    }
}
