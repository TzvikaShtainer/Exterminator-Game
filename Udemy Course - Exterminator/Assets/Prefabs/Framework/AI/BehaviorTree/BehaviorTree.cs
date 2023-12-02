using UnityEngine;

namespace Prefabs.Framework.AI.BehaviorTree
{
    public abstract class BehaviorTree : MonoBehaviour
    {
        private BTNode root; //current root that's Execute
        private BlackBoard blackboard = new BlackBoard();

        private IBehaviorTree behaviorTreeInterface; 
        //private BTNode prevNode; for debug down

        public BlackBoard Blackboard
        {
            get{
                return blackboard;
            }
        }
    
        private void Start()
        {
            behaviorTreeInterface = GetComponent<IBehaviorTree>(); //so that the other classes in the ConstructTree can retrieve it
            ConstructTree(out root);
            SortTree();
        }

        protected abstract void ConstructTree(out BTNode rootNode);
        
        private void SortTree()
        {
            int priortyCounter = 0;
            root.Initialize();
            root.SortPriority(ref priortyCounter);
        }
        
        private void Update()
        {
            root.UpdateNode();

            
            
            
            //debug to see the nodes:
            // BTNode currNode = root.GetNode();
            //
            // if (prevNode != currNode)
            // {
            //     prevNode = currNode;
            //     Debug.Log($"current node changed to: {currNode}");
            // }
        }

        public void AbortLowerThen(int priority)
        {
            BTNode currentNode = root.GetNode();
            if (currentNode.GetPriority() > priority)
            {
                root.Abort();
            }
        }

        public IBehaviorTree GetBehaviorTreeInterface()
        {
            return behaviorTreeInterface;
        }
    }
}
