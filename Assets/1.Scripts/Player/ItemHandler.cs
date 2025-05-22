
using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemHandler:MonoBehaviour
{
    private Player player;
    private ItemSlotUI itemSlotUI;

    [SerializeField]
    private int maxItemCount = 2;
    [SerializeField]
    private List<ItemData> items;
    private List<ItemEffectTimer> itemEffectTimers;
    private void Awake()
    {
        player = GetComponent<Player>();
        
        itemEffectTimers = new List<ItemEffectTimer>();
        items = new List<ItemData>();
    }

    private void Start()
    {
        itemSlotUI = GameManager.Instance.uiManager.itemSlotUI;
    }

    public void AddItem(ItemData item)
    {
        if (items.Count >= maxItemCount) return;
        
        items.Add(item);
        itemSlotUI.UpdateSlots(items);
    }
    public void UseItem(int index)
    {
        int trueIndex = index - 1;
        if (!items[trueIndex]) return;
        
        items[trueIndex].Use(player);
        items.RemoveAt(trueIndex);
        itemSlotUI.UpdateSlots(items);
    }
    
    public void AddItemEffectTimer(ItemEffectTimer timer)
    {
        itemEffectTimers.Add(timer);
    }

    public void RemoveItemEffectTimer(ItemEffectTimer timer)
    {
        itemEffectTimers.Remove(timer);
    }
}
