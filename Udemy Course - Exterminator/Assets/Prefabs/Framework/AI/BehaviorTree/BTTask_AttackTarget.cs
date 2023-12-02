using UnityEngine;

namespace Prefabs.Framework.AI.BehaviorTree
{
    public class BTTask_AttackTarget : BTNode
    {
        private BehaviorTree tree;
        private string targetKey;
        private GameObject target;

        public BTTask_AttackTarget(BehaviorTree tree, string targetKey)
        {
            this.tree = tree;
            this.targetKey = targetKey;
        }
        protected override NodeResult Execute()
        {
            if (!tree && tree.Blackboard == null || !tree.Blackboard.GetBlackboardData(targetKey, out target))
                return NodeResult.Failure;

            IBehaviorTree behaviorTree = tree.GetBehaviorTreeInterface();
            if (behaviorTree == null)
                return NodeResult.Failure;
            
            behaviorTree.AttackTarget(target);
            return NodeResult.Success;
        }
    }
}
