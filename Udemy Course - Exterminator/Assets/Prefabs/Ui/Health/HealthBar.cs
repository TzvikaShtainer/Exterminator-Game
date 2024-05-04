using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    private Transform attachPoint;

    public void Init(Transform attachPoint)
    {
        this.attachPoint = attachPoint;
    }
    
    public void SetHealthSliderValue(float health, float amt, float maxHealth)
    {
        healthSlider.value = health/maxHealth;
    }

    private void Update()
    {
        Vector3 ownerScreenPoint = Camera.main.WorldToScreenPoint(attachPoint.position);
        transform.position = ownerScreenPoint; //put the healthbar in the place we want reletive to the trans
    }

    public void OnOwnerDeath(GameObject killer)
    {
        Destroy(gameObject);
    }
}
