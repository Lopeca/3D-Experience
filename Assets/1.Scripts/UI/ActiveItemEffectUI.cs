using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveItemEffectUI : MonoBehaviour
{
    public GameObject itemEffectIconPrefab;

    private void Awake()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}