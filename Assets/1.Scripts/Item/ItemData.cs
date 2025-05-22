using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class ItemEffect : ScriptableObject
{
    protected Player player;
    protected Sprite icon;

    public virtual void Init(Player _player, Sprite _icon)
    {
        player = _player;
        icon = _icon;
    }
    public abstract void Activate();
    public abstract void Deactivate();
}


[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/ItemData")]
public class ItemData : ScriptableObject
{
    [SerializeField]
    private string itemName;
    public string ItemName => itemName;
    
    [SerializeField]
    private Sprite icon;
    public Sprite Icon => icon;

    [SerializeField]
    private ItemEffect effect;
    public ItemEffect Effect => effect;

    public void Use(Player player)
    {
        effect.Init(player, icon);
        effect.Activate();
    }
}
