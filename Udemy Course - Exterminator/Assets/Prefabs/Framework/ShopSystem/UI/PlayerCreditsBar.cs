using System;
using Prefabs.Ui.InGameUI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Prefabs.Framework.ShopSystem.UI
{
    public class PlayerCreditsBar : MonoBehaviour
    {
        [SerializeField] private CreditComponent creditComponent;
        [SerializeField] private Button shopBtn;
        [SerializeField] private TextMeshProUGUI creditText;
        [SerializeField] private UIManager uiManager;

        private void OnDisable()
        {
            creditComponent.onCreditsChanged -= UpdateCredit;
        }

        private void Start()
        {
            shopBtn.onClick.AddListener(PulloutShop);
            creditComponent.onCreditsChanged += UpdateCredit;
            UpdateCredit(creditComponent.Credit);
        }

        private void UpdateCredit(int newCredit)
        {
            creditText.SetText(newCredit.ToString());
        }

        private void PulloutShop()
        {
            uiManager.SwitchToShop();
        }
    }
}
