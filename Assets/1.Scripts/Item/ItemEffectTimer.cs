using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffectTimer : MonoBehaviour
{
    ItemEffect effect;
    private Sprite icon;
    private float duration;
    private float elapsedTime;

    public void Init(ItemEffect _effect, float _duration, Sprite _icon)
    {
        effect = _effect;
        duration = _duration;
        icon = _icon;
        elapsedTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= duration)
        {
            effect.Deactivate();
            Destroy(this);
        }
    }
}
