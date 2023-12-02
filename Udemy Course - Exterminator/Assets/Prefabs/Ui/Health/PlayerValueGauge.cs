using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Prefabs.Ui.Health
{
    public class PlayerValueGauge : MonoBehaviour
    {
        [SerializeField] private Image amtImage;
        [SerializeField] private TextMeshProUGUI amtText;


        public void UpdateValue(float health, float deltaSpeed, float maxHealth)
        {
            amtImage.fillAmount = health / maxHealth;
            int healthAsInt = (int)health;
            amtText.SetText(healthAsInt.ToString());
        }
    }
}
