namespace Prefabs.Framework.AI.BehaviorTree
{
    //The selector will run throw all the children from left to right and return the first child that will be successful.
    public class Selector : Compositor
    {
        protected override NodeResult Update()
        {
            NodeResult result = GetCurrentChild().UpdateNode();
            
            if (result == NodeResult.Success)
            {
                return NodeResult.Success;
            }
            
            if (result == NodeResult.Failure)
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
