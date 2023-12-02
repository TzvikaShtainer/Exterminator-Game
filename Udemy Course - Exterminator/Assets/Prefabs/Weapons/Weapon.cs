using Prefabs.Framework.Health;
using UnityEngine;

namespace Prefabs.Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField] private string attachSlotTag;
        [SerializeField] private float attackRateMult = 1f;
        [SerializeField] private AnimatorOverrideController overrideController;

        [SerializeField] private AudioClip weaponAudio;
        [SerializeField] private float weaponVol = 1f;
        private AudioSource weaponAudioSource;
        public GameObject Owner { get; private set; }

        private void Awake()
        {
            weaponAudioSource = GetComponent<AudioSource>();
        }

        public abstract void Attack();

        public void Init(GameObject owner)
        {
            Owner = owner;
            UnEquip();
        }

        public string GetAttachSlot()
        {
            return attachSlotTag;
        }
        public void Equip()
        {
            gameObject.SetActive(true);
            Owner.GetComponent<Animator>().runtimeAnimatorController = overrideController;
            Owner.GetComponent<Animator>().SetFloat("attackRateMult", attackRateMult); //set our fire rate throw the animator
        }

        public void UnEquip()
        {
            gameObject.SetActive(false);
        }

        public void DamageGameObject(GameObject obj, float amt)
        {
            HealthComponent healthComponent = obj.GetComponent<HealthComponent>();

            if (healthComponent != null)
            {
                healthComponent.ChangeHealth(-amt, Owner);
            }
        }

        protected void PlayWeaponAudio()
        {
            weaponAudioSource.PlayOneShot(weaponAudio, weaponVol);
        }
    }
}
