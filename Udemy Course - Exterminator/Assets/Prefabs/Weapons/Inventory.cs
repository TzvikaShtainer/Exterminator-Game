using System.Collections.Generic;
using Prefabs.Framework.ShopSystem;
using Unity.VisualScripting;
using UnityEngine;

namespace Prefabs.Weapons
{
    public class Inventory : MonoBehaviour, IPurchaseListener
    {
        [SerializeField] Weapon[] initialWeaponPrefabs;

        [SerializeField] Transform[] weaponSlots;
        [SerializeField] Transform defaultWeaponSlot;

        private List<Weapon> weapons = new List<Weapon>();
        private int currWeaponIndex = -1;

        private void Start()
        {
            InitWeapons();
        }

        private void InitWeapons()
        {
            foreach (Weapon weapon in initialWeaponPrefabs)
            {
                GiveNewWeapon(weapon);
            }
        
            NextWeapon();
        }

        private void GiveNewWeapon(Weapon weapon)
        {
            Transform weaponSlot = defaultWeaponSlot;
            foreach (var slot in weaponSlots)
            {
                if (slot.gameObject.CompareTag(weapon.GetAttachSlot()))
                {
                    weaponSlot = slot;
                }
            }

            Weapon newWeapon = Instantiate(weapon, weaponSlot);
            newWeapon.Init(gameObject);
            weapons.Add(newWeapon);
        }

        public void NextWeapon()
        {
            int nextWeaponIndex = currWeaponIndex + 1;
            if (nextWeaponIndex >= weapons.Count)
            {
                nextWeaponIndex = 0;
            }

            EquipWeapon(nextWeaponIndex);
        }

        private void EquipWeapon(int weaponIndex)
        {
            if (weaponIndex < 0 || weaponIndex >= weapons.Count)
                return;

            if (currWeaponIndex >= 0 && currWeaponIndex < weapons.Count)
            {
                weapons[currWeaponIndex].UnEquip();
            }
            weapons[weaponIndex].Equip();
            currWeaponIndex = weaponIndex;
        }

        public Weapon GetActiveWeapon()
        {
            if (hasWeapon())
            {
                return weapons[currWeaponIndex];
            }

            return null;
        }

        public bool HandlePurchase(Object newPurchase)
        {
            Object itemAsGameObject = newPurchase as GameObject;
            if(itemAsGameObject == null) return false;

            Weapon itemAsWeapon = itemAsGameObject.GetComponent<Weapon>();
            if(itemAsWeapon == null) return false;
            
            bool hasWeapon = weapons.Count != 0;
                
            GiveNewWeapon(itemAsWeapon);

            if (!hasWeapon)
            {
                EquipWeapon(0);
            }
            
            return true;
        }

        public bool hasWeapon()
        {
            return weapons.Count != 0;
        }
    }
}
