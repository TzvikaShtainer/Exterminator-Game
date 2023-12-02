using UnityEngine;

namespace Prefabs.Framework.AI.BehaviorTree
{
    public class BTTask_GetNextPatrolPoint : BTNode
    {
        private PatrollingComponent patrollingComponent;
        private BehaviorTree tree;
        private string patrolKey;
        
        public BTTask_GetNextPatrolPoint(BehaviorTree tree, string patrolKey)
        {
            patrollingComponent = tree.GetComponent<PatrollingComponent>();
            this.tree = tree;
            this.patrolKey = patrolKey;
        }

        protected override NodeResult Execute()
        {
            if (patrollingComponent != null && patrollingComponent.GetNextPatrolPoint(out Vector3 point))
            {
                tree.Blackboard.SetOrAddData(patrolKey, point);
                return NodeResult.Success;
            }

            return NodeResult.Failure;
        }
    }
}
