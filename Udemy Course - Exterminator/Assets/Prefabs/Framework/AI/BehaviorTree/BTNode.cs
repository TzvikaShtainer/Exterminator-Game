using UnityEngine;

public enum NodeResult
{
    Success,
    Failure, 
    Inprogress
}

namespace Prefabs.Framework.AI.BehaviorTree
{
    public abstract class BTNode
    { 
        bool started = false;
        private int priority;
        
        public NodeResult UpdateNode()
        {
            //one off thing
            if (!started)
            {
                started = true;
                NodeResult execResult = Execute();
                if (execResult != NodeResult.Inprogress)
                {
                    EndNode();
                    return execResult;
                }
            }
            
            //time based
            NodeResult updateResult = Update();
            if (updateResult != NodeResult.Inprogress)
            {
                EndNode();
            }
            
            return updateResult;
        }
        
        //override in child class
        protected virtual NodeResult Execute()
        {
            //one off thing
            return NodeResult.Success;
        }
        
        protected virtual NodeResult Update()
        {
            //time based
            return NodeResult.Success;
        }
        
        protected virtual void End()
        {
            //reset & clean up
        }
        
        private void EndNode()
        {
            started = false;
            End();
        }

        public void Abort()
        {
            EndNode();
        }

        public int GetPriority()
        {
            return priority;
        }

        public virtual void SortPriority(ref int priorityCounter)
        {
            priority = priorityCounter++;
            Debug.Log($"{this} has priority {priority}");
        }

        public virtual void Initialize()
        {
            
        }

        public virtual BTNode GetNode()
        {
            return this;
        }
    }
}
