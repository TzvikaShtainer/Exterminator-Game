using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Prefabs.Framework.ShopSystem.UI
{
    public class ShopItemUI : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI priceText;
        [SerializeField] private TextMeshProUGUI description;

        [SerializeField] private Button button;
        [SerializeField] private Image grayOutCover;

        private ShopItem item;

        [SerializeField] private Color inEfficientCreditColor;
        [SerializeField] private Color surEfficientCreditColor;

        public delegate void OnItemSelected(ShopItemUI selectedItem);
        public event OnItemSelected onItemSelected;

        private void Start()
        {
            button.onClick.AddListener(ItemSelected);
        }

        private void ItemSelected()
        {
            onItemSelected?.Invoke(this);
        }

        public void Init(ShopItem item, int availableCredits)
        {
            this.item = item;

            icon.sprite = item.itemIcon;
            titleText.text = item.title;
            priceText.text = item.price.ToString();
            description.text = this.item.description;

            Refresh(availableCredits);
        }

        public void Refresh(int availableCredits)
        {
            if (availableCredits < item.price)
            {
                grayOutCover.enabled = false;
                priceText.color = inEfficientCreditColor;
            }
            else
            {
                grayOutCover.enabled = true;
                priceText.color = surEfficientCreditColor;
            }
        }

        public ShopItem GetItem()
        {
            return item;
        }
    }
}