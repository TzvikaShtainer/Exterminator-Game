using System;
using Prefabs.Framework.Health;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Prefabs.Framework.Damage
{
    public class DamageVisualiser : MonoBehaviour
    {
        [SerializeField] private Renderer mesh;
        [SerializeField] private Color damageEmissionColor;
        [SerializeField] private float blinkSpeed = 2f;
        [SerializeField] private string emissionColorPropertyName = "_Addition";
        [SerializeField] private HealthComponent healthComponent;
        private Color originalEmissionColor;

        private void OnDisable()
        {
            healthComponent.onTakeDamage -= TookDamage;
        }

        private void Start()
        {
            Material mat = mesh.material;
            mesh.material = new Material(mat);

            originalEmissionColor = mesh.material.GetColor(emissionColorPropertyName);
            healthComponent.onTakeDamage += TookDamage;
        }

        protected virtual void TookDamage(float health, float amt, float maxHealth, GameObject instigator)
        {
            Color currentEmissionColor = mesh.material.GetColor(emissionColorPropertyName);
            if (Mathf.Abs((currentEmissionColor - originalEmissionColor).grayscale) < 0.1f)
            {
                mesh.material.SetColor(emissionColorPropertyName, damageEmissionColor);
            }
        }

        private void Update()
        {
            Color currentEmissionColor = mesh.material.GetColor(emissionColorPropertyName);
            Color newEmissionColor = Color.Lerp(currentEmissionColor, originalEmissionColor, Time.deltaTime * blinkSpeed);
            mesh.material.SetColor(emissionColorPropertyName, newEmissionColor);
        }
    }
}
