using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "HealEffect", menuName = "ScriptableObjects/ItemEffects/HealEffect")]
public class HealEffect : ItemEffect
{
    public override void Activate()
    {
        GameManager.Instance.player.Heal();
    }
    
    public override void Deactivate() { }
}