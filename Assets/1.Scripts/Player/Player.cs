using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public PlayerController playerController;
    public ItemHandler itemHandler;
    
    [Header("Stats")]
    [SerializeField]
    float maxHealth;
    public float MaxHealth => maxHealth;
    [SerializeField]
    float health; // 코드 복잡해지면 스탯 핸들러로 뺄 것

    [SerializeField]
    private float damageTerm = 1f;
    private float lastDamagedTime = 0;
    
    [SerializeField]
    //int invincibleVariety = 0; // 무적 아이템 수가 복수일 때를 가정
    
    [Header("UI")]
    private HurtAlertPanel hurtAlertPanel;
    private GageTypeUI healthUI;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        itemHandler = GetComponent<ItemHandler>();
    }

    private void Start()
    {
        healthUI = GameManager.Instance.uiManager.healthUI;
        hurtAlertPanel = GameManager.Instance.uiManager.hurtAlert;
    }

    public void TakeDamage(float damage)
    {
        if (lastDamagedTime + damageTerm < Time.time)
        {
            health -= damage;
            health = Mathf.Clamp(health, 0, 100);
            healthUI.SetValue(health);
            lastDamagedTime = Time.time;
            
            hurtAlertPanel.Alert();
        }
    }

    private void OnValidate()
    {
        healthUI?.SetValue(health);
    }
}