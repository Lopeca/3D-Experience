using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotCell : MonoBehaviour
{
    [SerializeField]
    private Image iconImage;
    
    public void Init()
    {
        iconImage.gameObject.SetActive(false);
    }

    public void SetIcon(Sprite sprite)
    {
        if (sprite)
        {
            iconImage.sprite = sprite;
            iconImage.gameObject.SetActive(true);
        }
        else
        {
            iconImage.gameObject.SetActive(false);
        }
    }
}
