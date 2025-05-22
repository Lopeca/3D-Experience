using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIForDebug : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI velocityText;

    // Update is called once per frame
    void Update()
    {
        velocityText.text = GameManager.Instance?.player.playerController.currentSpeed.ToString("F2");
    }
}
