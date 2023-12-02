using System;
using UnityEngine;

namespace Prefabs.Enemy.Spawner
{

    [Serializable]
    public class VFXSpec
    {
        public ParticleSystem particleSystem;
        public float size;
    }

public class Spawner : Enemy
{
    [SerializeField] private VFXSpec[] DeathVFX;
        protected override void Dead()
        {
            foreach (VFXSpec spec in DeathVFX)
            {
                ParticleSystem particleSystem = Instantiate(spec.particleSystem);
                particleSystem.transform.position = transform.position;
                particleSystem.transform.localScale = Vector3.one * spec.size;
            }
        }
    }
}
