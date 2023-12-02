using Prefabs.Framework.AI.BehaviorTree.BTTask_Groups;

namespace Prefabs.Framework.AI.BehaviorTree
{
    public class ChomperBehavior : BehaviorTree
    {
        //that's the part that everything go like program, here we build our tree
        protected override void ConstructTree(out BTNode rootNode)
        {
            //create our main compositor
            Selector rootSelector = new Selector();

            rootSelector.AddChild(new BtTaskGroupAttackTarget(this, 2, 10));
            //before groups tasks:
            #region Attack
            //--create our compositor
            //Sequencer attackTargetSequencer = new Sequencer();

            //--build the tasks
            //BTTask_MoveToTarget moveToTarget = new BTTask_MoveToTarget(this, "Target", 2);
            //BTTask_RotateTowardsTarget rotateTowardsTarget = new BTTask_RotateTowardsTarget(this, "Target", 10f);
            //BTTask_AttackTarget attackTarget = new BTTask_AttackTarget(this, "Target");
            
            //--add them to the relevant compositor
            //attackTargetSequencer.AddChild(moveToTarget);
            //attackTargetSequencer.AddChild(rotateTowardsTarget);
            //attackTargetSequencer.AddChild(attackTarget);
            
            //BlackboardDecorator attackTargetDecorator = new BlackboardDecorator(this, 
            //                                                                         attackTargetSequencer, 
            //                                                                         "Target", 
            //                                                                        BlackboardDecorator.RunCondition.KeyExists, 
            //                                                                         BlackboardDecorator.NotifyRule.RunConditionChange, 
            //                                                                         BlackboardDecorator.NotifyAbort.Both );
            //add the seq to the selector higher node
            
            //rootSelector.AddChild(attackTargetDecorator);
            #endregion 

            
            rootSelector.AddChild(new BTTask_Group_MoveToLastSeenLoc(this, 3));
            //before groups tasks:
            #region Go To Last Known Location Of Target
            // Sequencer checkLastSeenLocationSequencer = new Sequencer();
            //
            // BTTask_MoveToLocation moveToTargetLastSeenLocation = new BTTask_MoveToLocation(this, "LastSeenLocation", 3f);
            // BTTask_Wait waitInTargetLastSeenLocation= new BTTask_Wait(2f);
            // BTTask_RemoveBlackboardData removeLastSeenLocation = new BTTask_RemoveBlackboardData(this, "LastSeenLocation");
            //
            // checkLastSeenLocationSequencer.AddChild(moveToTargetLastSeenLocation);
            // checkLastSeenLocationSequencer.AddChild(waitInTargetLastSeenLocation);
            // checkLastSeenLocationSequencer.AddChild(removeLastSeenLocation);
            //
            // BlackboardDecorator checkLastSeenLocationDecorator = new BlackboardDecorator(this,
            //                                                                                 checkLastSeenLocationSequencer,
            //                                                                                 "LastSeenLocation",
            //                                                                                 BlackboardDecorator.RunCondition.KeyExists,
            //                                                                                 BlackboardDecorator.NotifyRule.RunConditionChange,
            //                                                                                 BlackboardDecorator.NotifyAbort.None);
            //
            // rootSelector.AddChild(checkLastSeenLocationDecorator);
            #endregion
            
            rootSelector.AddChild(new BTTask_Group_Patrolling(this, 3));
            //before groups tasks:
            #region Patrolling
            //--create our compositor 
            //Sequencer patrollingSequencer = new Sequencer();
            
            //--build the tasks
            //BTTask_GetNextPatrolPoint getNextPatrolPoint = new BTTask_GetNextPatrolPoint(this, "PatrolPoint");
            //BTTask_MoveToLocation moveToLocation = new BTTask_MoveToLocation(this, "PatrolPoint", 3);
            //BTTask_Wait taskWait = new BTTask_Wait(2f);
            
            //--add them to the relevant compositor
            //patrollingSequencer.AddChild(getNextPatrolPoint);
            //patrollingSequencer.AddChild(moveToLocation);
            //patrollingSequencer.AddChild(taskWait);
            
            //--add the seq to the selector higher node
            //rootSelector.AddChild(patrollingSequencer);
            

            #endregion
            
            
            //Execute the tree
            rootNode = rootSelector;

            //ExampleOfBoth(out rootNode); //down
        }

        public void ExampleOfBoth(out BTNode rootNode) 
        {
            //build the tasks
            BTTask_Wait waitTask = new BTTask_Wait(2f);
            BTTask_Log logTask = new BTTask_Log("logging");
            BTTask_AlwaysFail alwaysFail = new BTTask_AlwaysFail();

            
            
            //example of Sequencer - one fail and u out
            //create our compositor
            Sequencer rootSequencer = new Sequencer();
            
            //add them to the relevant compositor
            rootSequencer.AddChild(logTask);
            rootSequencer.AddChild(alwaysFail);     
            rootSequencer.AddChild(waitTask);
            
            //Execute the tree
            rootNode = rootSequencer;
            
            
            
            //example of selector - run success and u out
            //create our compositor
            Selector rootSelector = new Selector();
            //add them to the relevant compositor
            rootSelector.AddChild(alwaysFail);
            rootSelector.AddChild(logTask);
            rootSelector.AddChild(waitTask);
            
            //Execute the tree
            rootNode = rootSelector;
        }
    }
}
