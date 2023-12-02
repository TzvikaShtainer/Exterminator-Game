using UnityEngine;

namespace Prefabs.Weapons
{
    public class RangedWeapon : Weapon
    {
        [SerializeField] AimComponent aimComp;
        [SerializeField] private float damage = 10;
        [SerializeField] private ParticleSystem bulletVfx;
    
        public override void Attack()
        {
            GameObject target = aimComp.GetAimTarget(out Vector3 aimDir);
            Debug.Log(target);
        
            DamageGameObject(target, damage);
        
            bulletVfx.transform.rotation = Quaternion.LookRotation(aimDir); //makes the bullet follow the raycast line
            bulletVfx.Emit(bulletVfx.emission.GetBurst(0).maxCount);// makes the bullets
        
            PlayWeaponAudio();
        }
    }
}
