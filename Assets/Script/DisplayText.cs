using UnityEngine;
using UnityEngine.UI;

public class DisplayText : MonoBehaviour
{
    public Text displayText;
    public string message;

    void Start()
    {
        // 默认隐藏文本
        displayText.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        // 当玩家进入触发器范围时显示文本
        if (other.CompareTag("Player"))
        {
            displayText.text = message;
            displayText.enabled = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // 当玩家离开触发器范围时隐藏文本
        if (other.CompareTag("Player"))
        {
            displayText.enabled = false;
        }
    }
}
