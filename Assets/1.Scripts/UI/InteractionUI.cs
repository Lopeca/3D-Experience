using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractionUI : MonoBehaviour
{
    [SerializeField] RectTransform promptBG;
    [SerializeField] TextMeshProUGUI promptText;

    private float textPadding;

    private void Awake()
    {
        textPadding = promptText.rectTransform.position.x - promptBG.position.x;
    }

    public void ShowPrompt(string prompt)
    {
        promptText.text = prompt;
        LayoutRebuilder.ForceRebuildLayoutImmediate(promptText.rectTransform);
        promptBG.sizeDelta = promptText.rectTransform.sizeDelta + new Vector2(textPadding * 2, 0);
    }
    
    
}
