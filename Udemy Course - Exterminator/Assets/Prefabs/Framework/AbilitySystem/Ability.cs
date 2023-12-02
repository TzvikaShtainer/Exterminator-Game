using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Prefabs.Framework.AbilitySystem
{
    public abstract class Ability : ScriptableObject
    {
        [SerializeField] private Sprite abilityImage;
        [SerializeField] private float cooldownDuration = 2f;
        [SerializeField] private float staminaCost = 10f;
        
        private bool abilityOnCooldown = false;

        [Header("Audio")]
        [SerializeField] private AudioClip abilityAudio;
        [SerializeField] private float volume;
        
        private AbilityComponent abilityComponent;
        public AbilityComponent AbilityComp
        {
            get { return abilityComponent; }
            private set { abilityComponent = value; }
        }
        
        public delegate void OnCooldownStarted();
        public event OnCooldownStarted onCooldownStarted;
        
        internal void InitAbility(AbilityComponent abilityComponent)
        {
            this.abilityComponent = abilityComponent;
        }

        public abstract void ActivateAbility();

        //check all the conditions needed to activate the ability
        protected bool CommitAbility()
        {
            if(abilityOnCooldown) return false;
            
            if(abilityComponent == null || !abilityComponent.TryConsumeStamina(staminaCost))
                return false;

            StartAbilityCooldown();
            GamePlayStatics.PlayAudioAtPlayer(abilityAudio, volume);
            
            return true;
        }

        void StartAbilityCooldown()
        {
            abilityComponent.StartCoroutine(CooldownCoroutine());
        }

        IEnumerator CooldownCoroutine()
        {
            abilityOnCooldown = true;
            onCooldownStarted?.Invoke();
            yield return new WaitForSeconds(cooldownDuration);
            abilityOnCooldown = false;
        }

        public Sprite GetAbilityIcon()
        {
            return abilityImage;
        }

        public float GetCooldownDuration()
        {
            return cooldownDuration;
        }
    }
}
