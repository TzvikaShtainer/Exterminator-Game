using UnityEngine;

namespace Prefabs.Framework.AI.BehaviorTree.BTTask_Groups
{
    public class BTTask_Group_Patrolling : BTTask_Group
    {
        private float acceptableDistance;
        
        public BTTask_Group_Patrolling(BehaviorTree tree, float acceptableDistance = 3f) : base(tree)
        {
            this.acceptableDistance = acceptableDistance;
        }

        protected override void ConstructTree(out BTNode Root)
        {
            //create our compositor 
            Sequencer patrollingSequencer = new Sequencer();
            
            //build the tasks
            BTTask_GetNextPatrolPoint getNextPatrolPoint = new BTTask_GetNextPatrolPoint(tree, "PatrolPoint");
            BTTask_MoveToLocation moveToLocation = new BTTask_MoveToLocation(tree, "PatrolPoint", acceptableDistance);
            BTTask_Wait taskWait = new BTTask_Wait(2f);
            
            //add them to the relevant compositor
            patrollingSequencer.AddChild(getNextPatrolPoint);
            patrollingSequencer.AddChild(moveToLocation);
            patrollingSequencer.AddChild(taskWait);
            
            //add the seq to the selector higher node
            Root = patrollingSequencer;
        }
    }
}
