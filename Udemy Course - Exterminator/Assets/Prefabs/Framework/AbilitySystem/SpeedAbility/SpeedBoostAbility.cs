using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prefabs.Framework.AbilitySystem
{
    [CreateAssetMenu(menuName = "Ability/SpeedBoost")]
    public class SpeedBoostAbility : Ability
    {
        [Header("Speed")]
        [SerializeField] private float boostAmount = 20f;
        [SerializeField] private float boostDuration = 2f;

        private Player.Player player;
        public override void ActivateAbility()
        {
            if(!CommitAbility()) return;

            player = AbilityComp.GetComponent<Player.Player>();
            player.AddMoveSpeed(boostAmount);
            AbilityComp.StartCoroutine(ResetSpeed());
        }

        IEnumerator ResetSpeed()
        {
            yield return new WaitForSeconds(boostDuration);
            player.AddMoveSpeed(-boostAmount);
        }
    }
}
