using System.Collections.Generic;
using UnityEngine;

namespace Prefabs.Framework.AI.BehaviorTree
{
    public class BlackBoard
    {
        private Dictionary<string, object> blackboardData = new Dictionary<string, object>();

        public delegate void OnBlackboardValueChange(string key, object val);
        public event OnBlackboardValueChange onBlackboardValueChange;

        public void SetOrAddData(string key, object val)
        {
            if (blackboardData.ContainsKey(key))
            {
                blackboardData[key] = val;
            }
            else
            {
                blackboardData.Add(key, val);
            }
            
            onBlackboardValueChange?.Invoke(key, val);
        }

        public bool GetBlackboardData<T>(string key, out T val)
        {
            val = default(T);
            if (blackboardData.ContainsKey(key))
            {
                val = (T)blackboardData[key];
                return true;
            }

            return false;
        }

        public bool HasKey(string key)
        {
            return blackboardData.ContainsKey(key);
        }

        public void RemoveBlackboardData(string key)
        {
            blackboardData.Remove(key);
            onBlackboardValueChange?.Invoke(key, null);
        }
    }
}
