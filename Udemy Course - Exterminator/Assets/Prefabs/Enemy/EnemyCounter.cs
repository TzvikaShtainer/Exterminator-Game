using System;
using Prefabs.Framework.LevelManager;
using UnityEngine;

namespace Prefabs.Enemy
{
    public class EnemyCounter : MonoBehaviour
    {
        private static int enemyCount = 0;

        private void Start()
        {
            enemyCount++;
        }

        private void OnDestroy()
        {
            enemyCount--;

            if (enemyCount <= 0)
            {
                LevelManager.LevelFinished();
            }
        }
    }
}
