using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Prefabs.Framework.AbilitySystem.UI
{
    public class AbilityDock : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
    {
        [SerializeField] private AbilityComponent abilityComponent;
        [SerializeField] private RectTransform root;
        [SerializeField] private VerticalLayoutGroup layoutGroup;
        [SerializeField] private AbilityUI abilityUIPrefab;

        [SerializeField] private float scaleRange = 200f;
        
        [SerializeField] private float highlightSize = 1.5f;
        [SerializeField] private float scaleSpeed = 20f;

        private Vector3 goalScale = Vector3.one;
        
        private List<AbilityUI> abilityUIs = new List<AbilityUI>();

        private PointerEventData touchData;
        private AbilityUI highlightedAbility;

        private void Awake()
        {
            abilityComponent.onNewAbilityAdded += AddAbility;
        }

        private void OnDisable()
        {
            abilityComponent.onNewAbilityAdded -= AddAbility;
        }

        private void Update()
        {
            if (touchData != null)
            {
                GetUIUnderPointer(touchData, out highlightedAbility);
                ArrangeScale(touchData);
            }

            transform.localScale = Vector3.Lerp(transform.localScale, goalScale, Time.deltaTime * scaleSpeed);
        }

        private void ArrangeScale(PointerEventData pointerEventData)
        {
            if (scaleRange == 0) return;
            
            float touchVerticalPos = touchData.position.y;
            foreach (AbilityUI abilityUI in abilityUIs)
            {
                float abilityUIVerticalPos = abilityUI.transform.position.y;
                float distance = Mathf.Abs(touchVerticalPos - abilityUIVerticalPos);

                if (distance > scaleRange)
                {
                    abilityUI.SetScaleAmt(0);
                    continue;
                }
                
                float scaleAmt = (scaleRange - distance) / scaleRange;
                abilityUI.SetScaleAmt(scaleAmt);
            }
        }

        private void AddAbility(Ability newAbility)
        {
            AbilityUI newAbilityUI = Instantiate(abilityUIPrefab, root);
            newAbilityUI.Init(newAbility);
            abilityUIs.Add(newAbilityUI);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            touchData = eventData;
            goalScale = Vector3.one * highlightSize;
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            if (highlightedAbility)
            {
                highlightedAbility.ActivateAbility();
            }
            touchData = null;
            ResetScale();
            goalScale = Vector3.one;
        }

        private void ResetScale()
        {
            foreach (AbilityUI abilityUI in abilityUIs)
            {
                abilityUI.SetScaleAmt(0);
            }
        }

        bool GetUIUnderPointer(PointerEventData eventData, out AbilityUI abilityUI)
        {
            List<RaycastResult> findAbility = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, findAbility);

            abilityUI = null;
            foreach (RaycastResult result in findAbility)
            {
                abilityUI = result.gameObject.GetComponentInParent<AbilityUI>();
                if (abilityUI != null)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
