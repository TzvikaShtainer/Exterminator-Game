using Prefabs.Framework.AI.BehaviorTree;

namespace Prefabs.Enemy.Spawner
{
    public class SpawnerBehavior : BehaviorTree
    {
        protected override void ConstructTree(out BTNode rootNode)
        {
            BTTask_Spawn spawnTask = new BTTask_Spawn(this);
            CoolDownDecorator spawnCoolDownDecorator = new CoolDownDecorator(this, spawnTask, 2f);
            BlackboardDecorator spawnBBDecorator = new BlackboardDecorator(this, spawnCoolDownDecorator,"Target" , BlackboardDecorator.RunCondition.KeyExists, BlackboardDecorator.NotifyRule.RunConditionChange, BlackboardDecorator.NotifyAbort.Both);

            rootNode = spawnBBDecorator;
        }
    }
}
