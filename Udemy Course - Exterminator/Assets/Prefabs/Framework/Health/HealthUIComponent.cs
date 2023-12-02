using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Prefabs.Framework.Health
{
    public class HealthUIComponent : MonoBehaviour
    {
        [SerializeField] private HealthBar healthBarToSpawn;
        [SerializeField] private Transform healthBarAttachPoint;
        [SerializeField] private HealthComponent healthComponent;
        
        private void Start()
        {
            InGameUI inGameUI = FindObjectOfType<InGameUI>();
            HealthBar newHealthBar = Instantiate(healthBarToSpawn, inGameUI.transform);
            
            newHealthBar.Init(healthBarAttachPoint);
            
            healthComponent.onHealthChange += newHealthBar.SetHealthSliderValue; //dont get how the SetHealthSliderValue get the values right
            healthComponent.onDeath += newHealthBar.OnOwnerDeath;
        }
    }
}
