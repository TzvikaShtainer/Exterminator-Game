using Prefabs.Framework.AI.BehaviorTree;
using Prefabs.Framework.AI.BehaviorTree.BTTask_Groups;

namespace Prefabs.Enemy.Spiter.Assets
{
    public class SpiterBehavior : BehaviorTree
    {
        protected override void ConstructTree(out BTNode rootNode)
        {
            Selector rootSelector = new Selector();
        
            rootSelector.AddChild(new BtTaskGroupAttackTarget(this, 5, 10, 4));
        
            rootSelector.AddChild(new BTTask_Group_MoveToLastSeenLoc(this, 3));
        
            rootSelector.AddChild(new BTTask_Group_Patrolling(this, 3));
        
            rootNode = rootSelector;
        }
    }
}
