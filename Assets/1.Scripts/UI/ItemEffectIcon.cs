using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemEffectIcon : MonoBehaviour
{
    [SerializeField]
    private Image spriteImage;
    
    [SerializeField]
    private Image sliderImage;

    public void Init(Sprite icon)
    {
        spriteImage.sprite = icon;
        sliderImage.fillAmount = 0;
    }
    public void SetValue(float percentage)
    {
        sliderImage.fillAmount = percentage;
    }
}
