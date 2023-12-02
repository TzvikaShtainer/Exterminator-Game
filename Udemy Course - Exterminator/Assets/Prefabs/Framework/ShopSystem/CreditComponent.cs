using System;
using System.Collections.Generic;
using Prefabs.Framework.Reward;
using UnityEngine;
using UnityEngine.TextCore.Text;
using Object = UnityEngine.Object;

namespace Prefabs.Framework.ShopSystem
{
    public interface IPurchaseListener
    {
        public bool HandlePurchase(Object newPurchase);
    }
    public class CreditComponent : MonoBehaviour, IRewardListener   
    {
        [SerializeField] private int credits;
        [SerializeField] private Component[] purchaseListeners;
        private List<IPurchaseListener> purchaseListenerInterfaces = new List<IPurchaseListener>();

        public int Credit
        {
            get { return credits; }
        }
        
        public delegate void OnCreditsChanged(int newCredit);
        public event OnCreditsChanged onCreditsChanged;

        private void Start()
        {
            CollectPurchaseListeners();
        }

        private void CollectPurchaseListeners()
        {
            foreach (Component listener in purchaseListeners)
            {
                IPurchaseListener listenerInterface = listener as IPurchaseListener;
                if (listenerInterface != null)
                {
                    purchaseListenerInterfaces.Add(listenerInterface);
                }
            }
        }
        
        public bool Purchase(int price, Object item)
        {
            if (credits < price) return false;

            credits -= price;
            onCreditsChanged?.Invoke(credits);
            
            BroadcastPurchase(item);
            
            return true;
        }

        private void BroadcastPurchase(Object item)
        {
            foreach (IPurchaseListener purchaseListener in purchaseListenerInterfaces)
            {
                if (purchaseListener.HandlePurchase(item))
                {
                    return;
                }
            }
        }

        public void Reward(Reward.Reward reward)
        {
            credits += reward.creditReward;
            onCreditsChanged?.Invoke(credits);
        }
    }
}
