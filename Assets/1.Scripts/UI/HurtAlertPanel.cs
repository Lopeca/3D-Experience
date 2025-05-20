using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HurtAlertPanel : MonoBehaviour
{
    private Image redPanel;

    private const float startAlpha = 0.35f;
    private const float recoverSpeed = 0.3f;

    // Update is called once per frame

    private void Awake()
    {
        redPanel = GetComponent<Image>();
    }

    void Update()
    {
        if (redPanel.color.a <= 0) return;
        Color color = redPanel.color;
        color.a -= recoverSpeed * Time.deltaTime;
        redPanel.color = color;
    }

    public void Alert()
    {
        Color color = redPanel.color;
        color.a = startAlpha;
        redPanel.color = color;
    }
}
