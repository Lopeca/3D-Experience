using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlotUI : MonoBehaviour
{
    ItemSlotCell[] itemSlotCells;

    private void Awake()
    {
        itemSlotCells = GetComponentsInChildren<ItemSlotCell>();
        
        foreach (ItemSlotCell cell in itemSlotCells)
        {
            cell.Init();
        }
    }

    public void UpdateSlots(List<ItemData> items)
    {
        for (int i = 0; i < itemSlotCells.Length; i++)
        {
            ItemSlotCell cell = itemSlotCells[i];
            
            if(i >= items.Count) cell.Init();
            else
            {
                cell.SetIcon(items[i].Icon);
            }
        }
    }
}
