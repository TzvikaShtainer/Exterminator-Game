using UnityEngine;

namespace Prefabs.Framework.AI.BehaviorTree.BTTask_Groups
{
    public class BtTaskGroupAttackTarget : BTTask_Group
    {
        float moveAcceptableDistance;
        float rotationAcceptableRadius; 
        float attackCooldownDuration;
        
        public BtTaskGroupAttackTarget(BehaviorTree tree, float moveAcceptableDistance = 2f, float rotationAcceptableRadius = 10f, float attackCooldownDuration = 0) : base(tree)
        {
            this.moveAcceptableDistance = moveAcceptableDistance;
            this.rotationAcceptableRadius = rotationAcceptableRadius;
            this.attackCooldownDuration = attackCooldownDuration;
        }

        protected override void ConstructTree(out BTNode Root)
        {
            //create our compositor
            Sequencer attackTargetSequencer = new Sequencer();

            //build the tasks
            BTTask_MoveToTarget moveToTarget = new BTTask_MoveToTarget(tree, "Target", moveAcceptableDistance);
            BTTask_RotateTowardsTarget rotateTowardsTarget = new BTTask_RotateTowardsTarget(tree, "Target", rotationAcceptableRadius);
            BTTask_AttackTarget attackTarget = new BTTask_AttackTarget(tree, "Target");

            CoolDownDecorator attackCooldownDecorator = new CoolDownDecorator(tree, attackTarget, attackCooldownDuration);
            
            //add them to the relevant compositor
            attackTargetSequencer.AddChild(moveToTarget);
            attackTargetSequencer.AddChild(rotateTowardsTarget);
            attackTargetSequencer.AddChild(attackCooldownDecorator); //attackTarget is in the attackCooldownDecorator
            
            BlackboardDecorator attackTargetDecorator = new BlackboardDecorator(tree, 
                attackTargetSequencer, 
                "Target", 
                BlackboardDecorator.RunCondition.KeyExists, 
                BlackboardDecorator.NotifyRule.RunConditionChange, 
                BlackboardDecorator.NotifyAbort.Both );
            //add the seq to the selector higher node
            Root = attackTargetDecorator;
        }
    }
}
