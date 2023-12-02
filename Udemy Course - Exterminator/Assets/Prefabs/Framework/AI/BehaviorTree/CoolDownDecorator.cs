using UnityEngine;

namespace Prefabs.Framework.AI.BehaviorTree
{
    public class CoolDownDecorator : Decorator
    {
        private BehaviorTree tree;
        private float cooldownTime;
        private float lastExecutionTime = -1;
        private bool failOnCooldown;
            
        public CoolDownDecorator(BehaviorTree tree, BTNode child, float cooldownTime, bool failOnCooldown = false) : base(child)
        {
            this.cooldownTime = cooldownTime;
            this.failOnCooldown = failOnCooldown;
        }

        protected override NodeResult Execute()
        {
            if (cooldownTime == 0) return 
                NodeResult.Inprogress;
            
            //first execution
            if (lastExecutionTime == -1) //in the start the time is invalid
            {
                lastExecutionTime = Time.timeSinceLevelLoad;
                return NodeResult.Inprogress;
            }

            //cooldown not finished
            if (Time.timeSinceLevelLoad - lastExecutionTime <cooldownTime)
            {
                if (failOnCooldown)
                    return NodeResult.Failure;
                else
                    return NodeResult.Success;
            }
            
            //cooldown is finished since last time
            lastExecutionTime = Time.timeSinceLevelLoad;
            return NodeResult.Inprogress;
        }

        protected override NodeResult Update()
        {
            return GetChild().UpdateNode();
        }
    }
}
