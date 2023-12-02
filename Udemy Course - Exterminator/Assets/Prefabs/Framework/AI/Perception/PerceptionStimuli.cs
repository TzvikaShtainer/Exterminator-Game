using UnityEngine;

namespace Prefabs.Framework.AI.Perception
{
    public class PerceptionStimuli : MonoBehaviour
    {
        private void Start()
        { 
            SenseComponent.RegisterStimuli(this);
        }

        private void OnDestroy()
        {
            SenseComponent.UnRegisterStimuli(this);
        }
    }
}

