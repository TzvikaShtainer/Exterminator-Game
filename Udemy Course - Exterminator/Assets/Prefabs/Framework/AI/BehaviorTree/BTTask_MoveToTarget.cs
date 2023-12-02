using UnityEngine;
using UnityEngine.AI;

namespace Prefabs.Framework.AI.BehaviorTree
{
    public class BTTask_MoveToTarget : BTNode
    { 
        NavMeshAgent agent;
        private string targetkey;
        private GameObject target; 
        float acceptableDistance = 1f;
        private BehaviorTree tree;
        

        public BTTask_MoveToTarget(BehaviorTree tree, string targetKey, float acceptableDistance = 1f)
        {
            this.targetkey = targetKey;
            this.acceptableDistance = acceptableDistance;
            this.tree = tree;
        }

        protected override NodeResult Execute()
        {
            BlackBoard blackBoard = tree.Blackboard;
            if (blackBoard == null || !blackBoard.GetBlackboardData(targetkey, out target))
                return NodeResult.Failure;

            agent = tree.GetComponent<NavMeshAgent>();
            if (agent == null)
                return NodeResult.Failure;

            if (IsTargetInAcceptableDistance())
                return NodeResult.Success;

            blackBoard.onBlackboardValueChange += BlackboardValueChanged;

            agent.SetDestination(target.transform.position);
            agent.isStopped = false;
            return NodeResult.Inprogress;
        }
        
        bool IsTargetInAcceptableDistance()
        {
            return Vector3.Distance(target.transform.position, tree.transform.position) <= acceptableDistance;
        }
        
        private void BlackboardValueChanged(string key, object val)
        {
            if (key == targetkey)
            {
                target = (GameObject)val;
            }
        }

        protected override NodeResult Update()
        {
            if (target == null)
            {
                agent.isStopped = true;
                return NodeResult.Failure;
            }
            
            agent.SetDestination(target.transform.position);
            if (IsTargetInAcceptableDistance())
            {
                agent.isStopped = true;
                return NodeResult.Success;
            }
            
            return NodeResult.Inprogress;
        }

        protected override void End()
        {
            agent.isStopped = true;
            tree.Blackboard.onBlackboardValueChange -= BlackboardValueChanged;
            base.End();
        }
    }
}
