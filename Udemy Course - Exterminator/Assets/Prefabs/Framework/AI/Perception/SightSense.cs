using System.Collections;
using System.Collections.Generic;
using Prefabs.Framework.AI.Perception;
using UnityEngine;
using UnityEngine.Serialization;

public class SightSense : SenseComponent
{
    [SerializeField] private float sightDistance = 5;
    [SerializeField] private float sightHalfAngle = 5;
    [SerializeField] private float eyeSight = 1;
    protected override bool IsStimuliSensable(PerceptionStimuli stimuli)
    {
        float distance = Vector3.Distance(transform.position, stimuli.transform.position);
        if (distance > sightDistance)
            return false;

        Vector3 forwardDir = transform.forward;
        Vector3 stimuliDir = (stimuli.transform.position - transform.position).normalized;

        if (Vector3.Angle(forwardDir, stimuliDir) > sightHalfAngle)
            return false;

        if (Physics.Raycast(transform.position + Vector3.up * eyeSight, stimuliDir, out RaycastHit hitInfo, sightDistance))
        {
            if (hitInfo.collider.gameObject != stimuli.gameObject) //we see something else like wall
            {
                return false;
            }
        }

        return true;
    }

    protected override void DrawDebug()
    {
        base.DrawDebug();
        Gizmos.color = Color.black;
        Vector3 drawCenter = transform.position + Vector3.up * eyeSight;
        Gizmos.DrawWireSphere(drawCenter, sightDistance);

        Vector3 leftLimitDir = Quaternion.AngleAxis(sightHalfAngle, Vector3.up) * transform.forward;
        Vector3 rightLimitDir = Quaternion.AngleAxis(-sightHalfAngle, Vector3.up) * transform.forward;
        
        Gizmos.DrawLine(drawCenter, drawCenter + leftLimitDir * sightDistance);
        Gizmos.DrawLine(drawCenter, drawCenter + rightLimitDir * sightDistance);
        
        
    }
}
