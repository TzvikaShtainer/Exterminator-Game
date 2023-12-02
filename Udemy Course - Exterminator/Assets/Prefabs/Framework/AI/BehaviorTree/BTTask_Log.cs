using System.Collections;
using System.Collections.Generic;
using Prefabs.Framework.AI.BehaviorTree;
using UnityEngine;

public class BTTask_Log : BTNode
{
    private string massage;

    public BTTask_Log(string msg)
    {
        massage = msg;
    }
    protected override NodeResult Execute()
    {
        Debug.Log(massage);
        return NodeResult.Success;
    }
}
