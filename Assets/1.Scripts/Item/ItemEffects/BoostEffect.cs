using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "BoostEffect", menuName = "ScriptableObjects/ItemEffects/BoostEffect")]
public class BoostEffect : ItemEffect
{
    [SerializeField]
    private float duration;
    
    private ItemEffectTimer timer;
    
    public override void Activate()
    {
        timer = player.AddComponent<ItemEffectTimer>();
        timer.Init(this, duration, icon);

        player.itemHandler.AddItemEffectTimer(timer);
    }



    public override void Deactivate()
    {
        player.itemHandler.RemoveItemEffectTimer(timer);
    }
}