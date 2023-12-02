using System.Collections;
using Prefabs.Framework.Health;
using Unity.VisualScripting;
using UnityEngine;

namespace Prefabs.Framework.AbilitySystem
{
    [CreateAssetMenu(menuName = "Ability/HealthRegen")]
    public class HealthRegenAbility : Ability
    {
        [Header("Health")]
        [SerializeField] private float healthRegenAmt = 20;
        [SerializeField] private float healthRegenDuration = 2f;
        
        public override void ActivateAbility()
        {
            if(!CommitAbility()) return;
            
            HealthComponent healthComp = AbilityComp.GetComponent<HealthComponent>();
            if (healthComp != null)
            {
                if (healthRegenDuration == 0)
                {
                    healthComp.ChangeHealth(healthRegenAmt, AbilityComp.gameObject);
                    return;
                }
                AbilityComp.StartCoroutine(StartHealthRegen(healthRegenAmt, healthRegenDuration, healthComp));
            }
        }

        IEnumerator StartHealthRegen(float amt, float duration, HealthComponent healthComp)
        {
            float counter = duration;
            float regenRate = amt / duration;
            while (counter > 0)
            {
                counter -= Time.deltaTime;
                healthComp.ChangeHealth(regenRate * Time.deltaTime, AbilityComp.gameObject);
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
