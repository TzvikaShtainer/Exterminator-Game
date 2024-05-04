using System;
using Prefabs.Framework.Health;
using UnityEngine;

namespace Prefabs.Framework.Damage
{
    public class TriggerDamageComponent : DamageComponent
    {
        [SerializeField] private float damage;
        [SerializeField] private BoxCollider trigger;
        [SerializeField] private bool startedEnabled = false;

        public void SetDamageEnabled(bool enabled)
        {
            trigger.enabled = enabled;
        }

        private void Start()
        {
            SetDamageEnabled(startedEnabled);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(!ShouldDamage(other.gameObject))
                return;

            HealthComponent healthComponent = other.GetComponent<HealthComponent>();
            if(healthComponent != null)
                healthComponent.ChangeHealth(-damage, gameObject);
        }
    }
}
