using System.Collections.Generic;
using UnityEngine;

namespace Prefabs.Framework.AI.BehaviorTree
{
    public abstract class Compositor : BTNode
    {
        private LinkedList<BTNode> children = new LinkedList<BTNode>();
        private LinkedListNode<BTNode> currentChild = null;

        public void AddChild(BTNode newChild)
        {
            children.AddLast(newChild);
        }
        protected override NodeResult Execute()
        {
            if (children.Count == 0)
            {
                return NodeResult.Success;
            }

            currentChild = children.First;
            return NodeResult.Inprogress;
        }

        protected bool Next()
        {
            if (currentChild != children.Last)
            {
                currentChild = currentChild.Next;
                return true;
            }

            return false;
        }

        protected override void End()
        {
            if(currentChild == null) return;
            
            currentChild.Value.Abort();
            currentChild = null;
        }

        protected BTNode GetCurrentChild()
        {
            return currentChild.Value;
        }

        public override void SortPriority(ref int priorityCounter)
        {
            base.SortPriority(ref priorityCounter);
            foreach (var child in children)
            {
                child.SortPriority(ref priorityCounter);
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            foreach (var child in children)
            {
                child.Initialize();
            }
            
        }

        public override BTNode GetNode()
        {
            if (currentChild == null)
            {
                if (children.Count != 0)
                {
                    return children.First.Value.GetNode();
                }
                else
                {
                    return this;
                }
            }

            return currentChild.Value.GetNode();
        }
    }
}
