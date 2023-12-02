using System;
using System.Collections.Generic;
using Prefabs.Framework.AbilitySystem.UI;
using Prefabs.Framework.Reward;
using Prefabs.Framework.ShopSystem;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Prefabs.Framework.AbilitySystem
{
    public class AbilityComponent : MonoBehaviour , IPurchaseListener, IRewardListener
    {
        [SerializeField] private Ability[] initialAbilities;
        private List<Ability> abilities = new List<Ability>();

        public delegate void OnNewAbilityAdded(Ability newAbility);
        public delegate void OnStaminaChange(float newAmount, float maxAmount);

        public event OnNewAbilityAdded onNewAbilityAdded;
        public event OnStaminaChange onStaminaChange;

        [SerializeField] private float stamina = 200f;
        [SerializeField] private float maxStamina = 200f;

        private void Start()
        {
            foreach (Ability ability in initialAbilities)
            {
                GiveAbility(ability);
            }
        }

        public void BroadcastStaminaChangeImmediately()
        {
            onStaminaChange?.Invoke(stamina, maxStamina);
        }
        
        void GiveAbility(Ability ability)
        {
            Ability newAbility = Instantiate(ability);
            newAbility.InitAbility(this);
            abilities.Add(newAbility);
            onNewAbilityAdded?.Invoke(newAbility);
        }

        public void ActivateAbility(Ability abilityToActivate)
        {
            if (abilities.Contains(abilityToActivate))
            {
                abilityToActivate.ActivateAbility();
            }
        }

        float GetStamina()
        {
            return stamina;
        }

        public bool TryConsumeStamina(float staminaToConsume)
        {
            if (stamina <= staminaToConsume) return false;

            stamina -= staminaToConsume;
            BroadcastStaminaChangeImmediately();
            return true;
        }

        public bool HandlePurchase(Object newPurchase)
        {
            Ability itemAsAbility = newPurchase as Ability;
            if (itemAsAbility == null) return false;
            
            GiveAbility(itemAsAbility);
            
            return true;
        }

        public void Reward(Reward.Reward reward)
        {
            stamina = Mathf.Clamp(stamina + reward.staminaReward, 0, maxStamina);
            BroadcastStaminaChangeImmediately();
        }
    }
}
