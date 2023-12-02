using Prefabs.Framework.AI.BehaviorTree;
using Prefabs.Framework.Damage;
using Unity.VisualScripting;
using UnityEngine;

namespace Prefabs.Enemy.Chomper.Assets
{
    public class Chomper : Enemy
    {
        [SerializeField] private TriggerDamageComponent damageComp;
        
        
        public override void AttackTarget(GameObject target)
        {
            Animator.SetTrigger("Attack");
        }

        public void AttackPoint()
        {
            if (damageComp)
            {
                damageComp.SetDamageEnabled(true);
            }
        }

        public void AttackEnd()
        {
            if (damageComp)
            {
                damageComp.SetDamageEnabled(false);
            }
        }

        protected override void Start()
        {
            base.Start();
            
            damageComp.SetTeamInterfaceSrc(this);
        }
    }
}
