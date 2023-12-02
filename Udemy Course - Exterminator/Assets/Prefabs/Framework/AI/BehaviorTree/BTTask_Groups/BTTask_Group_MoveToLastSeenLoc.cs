using UnityEngine;

namespace Prefabs.Framework.AI.BehaviorTree.BTTask_Groups
{
    public class BTTask_Group_MoveToLastSeenLoc : BTTask_Group
    {
        private float acceptableDistance;
        
        public BTTask_Group_MoveToLastSeenLoc(BehaviorTree tree, float acceptableDistance = 3f) : base(tree)
        {
            this.acceptableDistance = acceptableDistance;
        }

        protected override void ConstructTree(out BTNode Root)
        {
            //create our compositor
            Sequencer checkLastSeenLocationSequencer = new Sequencer();
            
            //build the tasks
            BTTask_MoveToLocation moveToTargetLastSeenLocation = new BTTask_MoveToLocation(tree, "LastSeenLocation", acceptableDistance);
            BTTask_Wait waitInTargetLastSeenLocation= new BTTask_Wait(2f);
            BTTask_RemoveBlackboardData removeLastSeenLocation = new BTTask_RemoveBlackboardData(tree, "LastSeenLocation");
            
            //add them to the relevant compositor
            checkLastSeenLocationSequencer.AddChild(moveToTargetLastSeenLocation);
            checkLastSeenLocationSequencer.AddChild(waitInTargetLastSeenLocation);
            checkLastSeenLocationSequencer.AddChild(removeLastSeenLocation);

            BlackboardDecorator checkLastSeenLocationDecorator = new BlackboardDecorator(tree,
                checkLastSeenLocationSequencer,
                "LastSeenLocation",
                BlackboardDecorator.RunCondition.KeyExists,
                BlackboardDecorator.NotifyRule.RunConditionChange,
                BlackboardDecorator.NotifyAbort.None);
            
            //add the seq to the selector higher node
            Root = checkLastSeenLocationDecorator;
        }
    }
}
