using UnityEngine;

namespace Prefabs.Framework.AI.Perception
{
    public class AlwaysAwareSense : SenseComponent
    {
        [SerializeField] private float awareDistance = 2f;
        protected override bool IsStimuliSensable(PerceptionStimuli stimuli)
        {
            return Vector3.Distance(transform.position, stimuli.transform.position) <= awareDistance;
        }

        protected override void DrawDebug()
        {
            base.DrawDebug();
            Gizmos.DrawWireSphere(transform.position + Vector3.up, awareDistance);
        }
    }
}
