namespace Prefabs.Framework.AI.BehaviorTree
{
    //The sequencer will run throw all the children from left to right and will return success only if all
    //the child’s return success, if one of them failed it will failed the all process and won’t proceed to the next child.
    public class Sequencer : Compositor
    {
        protected override NodeResult Update()
        {
            NodeResult result = GetCurrentChild().UpdateNode();

            if (result == NodeResult.Failure)
            {
                return NodeResult.Failure;
            }

            if (result == NodeResult.Success)
            {
                if (Next())
                    return NodeResult.Inprogress;
                else
                    return NodeResult.Success;
            }

            return NodeResult.Inprogress;
        }
    }
}
