namespace Prefabs.Framework.AI.BehaviorTree.BTTask_Groups
{
    public abstract class BTTask_Group : BTNode
    {
        private BTNode Root;
        protected BehaviorTree tree;

        public BTTask_Group(BehaviorTree tree)
        {
            this.tree = tree; 
        }

        protected abstract void ConstructTree(out BTNode Root);

        protected override NodeResult Execute()
        {
            return NodeResult.Inprogress;
        }

        protected override NodeResult Update()
        {
            return Root.UpdateNode();
        }

        protected override void End()
        {
            Root.Abort();
            base.End();
        }

        public override void SortPriority(ref int priorityCounter)
        {
            base.SortPriority(ref priorityCounter);
            Root.SortPriority(ref priorityCounter);
        }

        public override void Initialize()
        {
            base.Initialize();
            ConstructTree(out Root);
        }

        public override BTNode GetNode()
        {
            return Root.GetNode();
        }
    }
}
