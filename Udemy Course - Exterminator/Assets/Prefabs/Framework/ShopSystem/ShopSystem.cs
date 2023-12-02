using UnityEngine;

namespace Prefabs.Framework.ShopSystem
{
    [CreateAssetMenu(menuName = "Shop/ShopSystem")]
    public class ShopSystem : ScriptableObject
    {
        [SerializeField] private ShopItem[] shopItems;

        public ShopItem[] GetShopItems()
        {
            return shopItems;
        }

        public bool TryPurchase(ShopItem itemSelected, CreditComponent purchaser)
        {
            return purchaser.Purchase(itemSelected.price, itemSelected.item);
        }
    }
}
