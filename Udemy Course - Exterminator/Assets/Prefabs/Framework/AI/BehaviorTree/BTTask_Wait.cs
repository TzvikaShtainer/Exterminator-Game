using UnityEngine;

namespace Prefabs.Framework.AI.BehaviorTree
{
    public class BTTask_Wait : BTNode
    {
        private float waitTime = 2f;
        private float timeElapsed = 0f;

        public BTTask_Wait(float waitTime)
        {
            this.waitTime = waitTime;
        }
        protected override NodeResult Execute()
        {
            if (waitTime <= 0)
            {
                return NodeResult.Success;
            }
            //Debug.Log($"Wait Started with dur: {waitTime}");
            timeElapsed = 0f;
            
            return NodeResult.Inprogress;
        }

        protected override NodeResult Update()
        {
            timeElapsed += Time.deltaTime;
            if (timeElapsed >= waitTime)
            {
                //Debug.Log("wait finished");
                return NodeResult.Success;
            }
            //Debug.Log($"waiting for {timeElapsed}");
            return NodeResult.Inprogress;
        }
    }
}
