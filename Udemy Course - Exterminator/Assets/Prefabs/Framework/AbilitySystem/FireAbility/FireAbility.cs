using System.Collections;
using System.Collections.Generic;
using Prefabs.Framework.Damage;
using Prefabs.Framework.Health;
using UnityEngine;

namespace Prefabs.Framework.AbilitySystem
{
    [CreateAssetMenu(menuName = "Ability/Fire")]
    public class FireAbility : Ability
    {
        [Header("Fire")]
        [SerializeField] private Scanner scannerPrefab;
        [SerializeField] private float fireRadius;
        [SerializeField] private float fireDuration;
        
        [SerializeField] private float damageDuration;
        [SerializeField] private float fireDamage;
        
        [SerializeField] private GameObject scanVFX; //mybe its a collection and not just 1 particle
        [SerializeField] private GameObject damageVFX;
        public override void ActivateAbility()
        {
            if(!CommitAbility()) return;

            Scanner fireScanner = Instantiate(scannerPrefab, AbilityComp.transform);
            fireScanner.SetScanRange(fireRadius);
            fireScanner.SetScanDuration(fireDuration);
            fireScanner.AddChildAttached(Instantiate(scanVFX).transform);
            fireScanner.onScanDetectionUpdated += DetectionUpdate;
            fireScanner.StartScan();
        }

        private void DetectionUpdate(GameObject newDetection)
        {
            ITeamInterface detectedTeamInterface = newDetection.GetComponent<ITeamInterface>();
            if (detectedTeamInterface == null || detectedTeamInterface.GetRelationTowards(AbilityComp.gameObject) != ETeamRelation.Enemy)
            {
                return;
            }

            HealthComponent enemyHealthComp = newDetection.GetComponent<HealthComponent>();
            if (enemyHealthComp == null)
            {
                return;
            }
            
            AbilityComp.StartCoroutine(ApplyDamageTo(enemyHealthComp));
        }

        private IEnumerator ApplyDamageTo(HealthComponent enemyHealthComp)
        {
            GameObject newDamageVFX = Instantiate(damageVFX, enemyHealthComp.transform);
            float damageRate = fireDamage / damageDuration;
            float startTime = 0;
            while (startTime < damageDuration && enemyHealthComp != null)
            {
                //Debug.Log("here2");
                startTime += Time.deltaTime;
                enemyHealthComp.ChangeHealth(-damageRate * Time.deltaTime, AbilityComp.gameObject);
                yield return new WaitForEndOfFrame();
            }

            if (newDamageVFX != null)
            {
                Destroy(newDamageVFX);
            }
        }
    }
}
