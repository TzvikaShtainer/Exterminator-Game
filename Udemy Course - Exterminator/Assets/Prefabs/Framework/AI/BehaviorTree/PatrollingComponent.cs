using UnityEngine;

namespace Prefabs.Framework.AI.BehaviorTree
{
    public class PatrollingComponent : MonoBehaviour
    {
        [SerializeField] private Transform[] patrolPoints;
        [SerializeField] private int currentPatrolPointIndex = -1;

        public bool GetNextPatrolPoint(out Vector3 point)
        {
            point = Vector3.zero;
            
            if (patrolPoints.Length == 0) return false;
            
            currentPatrolPointIndex = (currentPatrolPointIndex + 1) % patrolPoints.Length; //circle the patrol arry
            point = patrolPoints[currentPatrolPointIndex].position;
            
            return true;
        }
    }
}
