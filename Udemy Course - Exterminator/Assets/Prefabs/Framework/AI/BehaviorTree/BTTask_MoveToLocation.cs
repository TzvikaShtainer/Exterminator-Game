using UnityEngine;
using UnityEngine.AI;

namespace Prefabs.Framework.AI.BehaviorTree
{
    public class BTTask_MoveToLocation : BTNode
    {
        //very similar to BTTask_MoveToTarget so its better to make a base class but its for later
        
        NavMeshAgent agent;
        private string locationKey;
        private Vector3 location; 
        float acceptableDistance;
        private BehaviorTree tree;
        
        public BTTask_MoveToLocation(BehaviorTree tree, string locationKey, float acceptableDistance = 1f)
        {
            this.locationKey = locationKey;
            this.acceptableDistance = acceptableDistance;
            this.tree = tree;
        }

        protected override NodeResult Execute()
        {
            BlackBoard blackboard = tree.Blackboard;
            if (blackboard == null || !blackboard.GetBlackboardData(locationKey, out location))
                return NodeResult.Failure;

            agent = tree.GetComponent<NavMeshAgent>();
            if (agent == null)
                return NodeResult.Failure;
            
            if (IsLocationInAcceptableDistance())
                return NodeResult.Success;

            agent.SetDestination(location);
            agent.isStopped = false;

            return NodeResult.Inprogress;
        }

        private bool IsLocationInAcceptableDistance()
        {
            return Vector3.Distance(location, tree.transform.position) <= acceptableDistance;
        }

        protected override NodeResult Update()
        {
            if (IsLocationInAcceptableDistance())
            {
                agent.isStopped = tree;
                return NodeResult.Success;
            }

            return NodeResult.Inprogress;
        }
        
        protected override void End()
        {
            agent.isStopped = true;
            base.End();
        }
    }
}
