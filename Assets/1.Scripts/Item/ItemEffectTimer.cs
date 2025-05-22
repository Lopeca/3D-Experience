using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffectTimer : MonoBehaviour
{
    ItemEffect effect;
    private ItemEffectIcon iconUI;

    private float duration;
    private float elapsedTime;

    public void Init(ItemEffect _effect, float _duration, Sprite _icon)
    {
        ActiveItemEffectUI effectUI = GameManager.Instance.uiManager.activeItemEffectUI;
        GameObject iconGO = Instantiate(effectUI.itemEffectIconPrefab, effectUI.transform);
        
        iconUI = iconGO.GetComponent<ItemEffectIcon>();
        iconUI.Init(_icon);
        
        effect = _effect;
        duration = _duration;
        elapsedTime = 0;
        
        //과제가 코루틴 사용이 전제라서 타이머 종료를 코루틴으로 구현해둡니다
        StartCoroutine(EndTimer());
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        iconUI?.SetValue(elapsedTime/duration);
    }

    IEnumerator EndTimer()
    {
        yield return new WaitForSeconds(duration);
        effect.Deactivate();
        Destroy(iconUI.gameObject);
        Destroy(this);
    }
}
