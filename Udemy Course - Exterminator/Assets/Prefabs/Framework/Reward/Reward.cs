using System;
using UnityEngine;

namespace Prefabs.Framework.Reward
{
    [Serializable]
    public class Reward
    {
        public int healthReward;
        public int creditReward;
        public int staminaReward;
    }

    public interface IRewardListener
    {
        public void Reward(Reward reward);
    }
}
