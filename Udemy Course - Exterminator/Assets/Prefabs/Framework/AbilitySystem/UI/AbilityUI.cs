using System;
using System.Collections;
//using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;
using Image = UnityEngine.UI.Image;

namespace Prefabs.Framework.AbilitySystem.UI
{
    public class AbilityUI : MonoBehaviour
    {
        private Ability ability;
        [SerializeField] private Image abilityIcon;
        [SerializeField] private Image cooldownWheel;
        
        [SerializeField] private float highlightSize = 1.5f;
        [SerializeField] private float highlightOffSet = 200f;
        [SerializeField] private float scaleSpeed = 20f;
        [SerializeField] private RectTransform offSetPivot;

        private Vector3 goalScale = Vector3.one;
        private Vector3 goalOffSet = Vector3.zero;

        private bool bIsOnCooldown = false;
        private float cooldownCounter = 0;

        private void OnDisable()
        {
            ability.onCooldownStarted -= StartCooldown;
        }

        private void Update()
        {
            transform.localScale = Vector3.Lerp(transform.localScale, goalScale, Time.deltaTime * scaleSpeed);
            offSetPivot.localPosition = Vector3.Lerp(offSetPivot.localPosition, goalScale,Time.deltaTime * scaleSpeed);
        }

        public void Init(Ability newAbility)
        {
            ability = newAbility;
            abilityIcon.sprite = newAbility.GetAbilityIcon();
            cooldownWheel.enabled = false;
            ability.onCooldownStarted += StartCooldown;
        }

        private void StartCooldown()
        {
            if (bIsOnCooldown) return;
            
            StartCoroutine(CooldownCoroutine());
        }

        IEnumerator CooldownCoroutine()
        {
            bIsOnCooldown = true;
            cooldownCounter = ability.GetCooldownDuration();
            float cooldownDuration = cooldownCounter; //need to have the full duration for the % clac
            cooldownWheel.enabled = true;
            

            while (cooldownCounter > 0)
            {
                cooldownCounter -= Time.deltaTime;
                cooldownWheel.fillAmount = cooldownCounter / cooldownDuration;
                yield return new WaitForEndOfFrame();
            }

            bIsOnCooldown = false;
            cooldownWheel.enabled = false;
        }

        public void ActivateAbility()
        {
            ability.ActivateAbility();
        }

        public void SetScaleAmt(float amt)
        {
            goalScale = Vector3.one * (1 + (highlightSize - 1) * amt);
            goalOffSet = Vector3.left * highlightOffSet * amt;
        }
    }
}
