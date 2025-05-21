
using UnityEngine;
using UnityEngine.UI;

public class GageTypeUI : MonoBehaviour
{
    public Image gage;
    private float maxValue;
    private float curValue;
    private float targetValue;
    
    
    // Start is called before the first frame update
    void Start()
    {
        maxValue = GameManager.Instance.player.MaxHealth;
        curValue = maxValue;
        targetValue = maxValue;
    }

    // Update is called once per frame
    void Update()
    {
        curValue = Mathf.Lerp(curValue, targetValue, 5 * Time.deltaTime);
        gage.fillAmount = curValue / maxValue;
    }

    public void AddValue(float value)
    {
        targetValue += value;
    }

    public void SetValue(float value)
    {
        targetValue = value;
    }
}
