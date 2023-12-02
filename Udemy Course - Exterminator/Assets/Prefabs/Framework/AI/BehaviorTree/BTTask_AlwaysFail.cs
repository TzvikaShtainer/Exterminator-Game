using System;
using System.Collections;
using System.Collections.Generic;
using Prefabs.Framework.AI.BehaviorTree;
using UnityEngine;

public class BTTask_AlwaysFail : BTNode
{
    protected override NodeResult Execute()
    {
        Debug.Log("Failed");
        return NodeResult.Failure;
    }
}
