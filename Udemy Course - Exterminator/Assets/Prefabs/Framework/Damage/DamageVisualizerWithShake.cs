using Prefabs.Framework.Camera;
using UnityEngine;

namespace Prefabs.Framework.Damage
{
    public class DamageVisualizerWithShake : DamageVisualiser
    {
        [SerializeField] private Shaker shaker;

        protected override void TookDamage(float health, float amt, float maxHealth, GameObject instigator)
        {
            base.TookDamage(health, amt, maxHealth, instigator);
            if (shaker != null)
            {
                shaker.StartShake();
            }
        }
    }
}
