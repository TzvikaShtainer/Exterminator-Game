using System;
using UnityEngine;

namespace Prefabs.Framework.AI.BehaviorTree
{
    public class BlackboardDecorator : Decorator
    {
        public enum RunCondition //to know if to act in the first place
        {
            KeyExists,
            KeyNotExists
        }
    
        public enum NotifyRule//to know if to act if something changed while we are doing a condition
        {
            RunConditionChange,
            KeyValueChange
        }

        public enum NotifyAbort //if the NotifyRule changed we need to notify it
        {
            None,
            Self,
            Lower,
            Both
        }
        
        private BehaviorTree tree;
        private string key;
        private object value;
        
        private RunCondition runCondition;
        private NotifyRule notifyRule;
        private NotifyAbort notifyAbort;
        
        public BlackboardDecorator(BehaviorTree tree, BTNode child, string key, RunCondition runCondition, NotifyRule notifyRule, NotifyAbort notifyAbort) : base(child)
        {
            this.tree = tree;
            this.key = key;
            this.runCondition = runCondition;
            this.notifyRule = notifyRule;
            this.notifyAbort = notifyAbort;
        }

        protected override NodeResult Execute()
        {
            BlackBoard blackBoard = tree.Blackboard;
            if (blackBoard == null)
                return NodeResult.Failure;

            blackBoard.onBlackboardValueChange -= CheckNotify;
            blackBoard.onBlackboardValueChange += CheckNotify; //to guarantee that we have only 1 CheckNotify
                                                                //we unsub first to eliminate the option to have 2 events
                                                                //and to cut the chain of action that we do right now

            if (CheckRunCondition())
            {
                return NodeResult.Inprogress;
            }
            else
            {
                return NodeResult.Failure;
            }
        }
        
        private void CheckNotify(string key, object val)
        {
            if (this.key != key) return;

            if (notifyRule == NotifyRule.RunConditionChange)
            {
                bool prevExists = value != null; //if the key exists before in the blackboard
                bool currentExists = val != null; //if the key exists now in the blackboard

                if (prevExists != currentExists) //condition changed
                {
                    Notify();
                }
            }
            else if (notifyRule == NotifyRule.KeyValueChange)
            {
                if (val != value)
                {
                    Notify();
                }
            }
        }

        private void Notify()
        {
            switch (notifyAbort)
            {
                case NotifyAbort.None:
                    break;
                case NotifyAbort.Self:
                    AbortSelf();
                    break;
                case NotifyAbort.Lower:
                    AbortLower();
                    break;
                case NotifyAbort.Both:
                    AbortBoth();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void AbortSelf()
        {
            Abort();
        }

        private void AbortLower()
        {
            tree.AbortLowerThen(GetPriority());
        }

        private void AbortBoth()
        {
            Abort();
            AbortLower();
        }

        private bool CheckRunCondition()
        {
            bool exists = tree.Blackboard.GetBlackboardData(key, out value);
            switch (runCondition)
            {
                case RunCondition.KeyExists:
                    return exists;
                case RunCondition.KeyNotExists:
                    return !exists;
            }
            
            return false;
        }

        protected override NodeResult Update()
        {
            return GetChild().UpdateNode();
        }

        protected override void End()
        {
            GetChild().Abort();
            base.End();
        }
    }
}
