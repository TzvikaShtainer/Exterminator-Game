using UnityEngine;

namespace Prefabs.Framework.ShopSystem
{
    [CreateAssetMenu(menuName = "Shop/ShopItem")]
    public class ShopItem : ScriptableObject
    {
        public string title;
        public int price;
        public Object item;
        public Sprite itemIcon;
        [TextArea(10, 10)]
        public string description;
    }
}
