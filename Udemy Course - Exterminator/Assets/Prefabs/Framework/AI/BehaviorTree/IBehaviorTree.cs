using UnityEngine;

namespace Prefabs.Framework.AI.BehaviorTree
{
    public interface IBehaviorTree
    {
        public void RotateTowards(GameObject target, bool verticalAim=false);
        public void AttackTarget(GameObject target);
    }
}
