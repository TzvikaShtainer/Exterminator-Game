using Prefabs.Framework.Reward;
using UnityEngine;

namespace Prefabs.Framework.Health
{
    public class HealthComponent : MonoBehaviour, IRewardListener
    {
        [SerializeField] private float health;
        [SerializeField] private float maxHealth;
    
        public delegate void OnHealthChange(float health, float amt, float maxHealth);
        public delegate void OnTakeDamage(float health, float amt, float maxHealth, GameObject instigator);
        public delegate void OnDeath(GameObject killer);

        public event OnHealthChange onHealthChange;
        public event OnTakeDamage onTakeDamage;
        public event OnDeath onDeath;


        [Header("Audio")]
        [SerializeField] private AudioClip hitAudio;
        [SerializeField] private AudioClip deathAudio;
        [SerializeField] private float volume;
        private AudioSource audioSrc;
        
        public void BroadcastHealthValueImmediately()
        {
            onHealthChange?.Invoke(health, 0, maxHealth);
        }

        public void ChangeHealth(float amt, GameObject instigator)
        {
            if (amt == 0 || health == 0) return;
        
            health += amt;

            if (health >= maxHealth)
            {
                health = maxHealth;
            }

            if (amt < 0)
            {
                onTakeDamage?.Invoke(health, amt, maxHealth, instigator);
                
                Vector3 loc = transform.position;
                GamePlayStatics.PlayAudioAtLoc(hitAudio, loc, volume);
            }
        
            onHealthChange?.Invoke(health, amt, maxHealth);

            if (health <= 0)
            {
                health = 0;
                onDeath?.Invoke(instigator);
                
                Vector3 loc = transform.position;
                
                if (!audioSrc.isPlaying)
                {
                    audioSrc.PlayOneShot(hitAudio, volume);
                }
            }
        }

        public void Reward(Reward.Reward reward)
        {
            health = Mathf.Clamp(health + reward.healthReward, 0, maxHealth);
            onHealthChange?.Invoke(health, reward.healthReward, maxHealth);
        }
    }
}

