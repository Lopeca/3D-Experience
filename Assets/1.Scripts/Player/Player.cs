using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    float maxHealth;
    public float MaxHealth => maxHealth;
    [SerializeField]
    float health; // 코드 복잡해지면 스탯 핸들러로 뺄 것

    private GageTypeUI healthUI;

    [SerializeField]
    private float damageTerm = 1f;
    private float lastDamagedTime = 0;
    [SerializeField]
    private HurtAlertPanel hurtAlert;
    private void Start()
    {
        healthUI = GameManager.Instance.uiManager.healthUI;
        hurtAlert = GameManager.Instance.uiManager.hurtAlert;
    }

    public void TakeDamage(float damage)
    {
        if (lastDamagedTime + damageTerm < Time.time)
        {
            health -= damage;
            health = Mathf.Clamp(health, 0, 100);
            healthUI.SetValue(health);
            lastDamagedTime = Time.time;
            
            hurtAlert.Alert();
        }
    }

    private void OnValidate()
    {
        healthUI?.SetValue(health);
    }
}