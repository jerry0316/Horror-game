using UnityEngine;
using TMPro;

public class FinalKey : MonoBehaviour
{
    public FinalDoor finalDoor;  // 引用FinalDoor腳本
    public TextMeshProUGUI pickupText;  // 使用TextMeshProUGUI類型
    private bool isPlayerInRange = false;  // 檢查玩家是否在範圍內

    void Start()
    {
        // 初始隱藏提示文字
        if (pickupText != null)
        {
            pickupText.enabled = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;  // 玩家進入範圍
            if (pickupText != null)
            {
                pickupText.text = "按F撿起鑰匙";  // 顯示提示文字
                pickupText.enabled = true;  // 顯示Text元件
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;  // 玩家離開範圍
            if (pickupText != null)
            {
                pickupText.enabled = false;  // 隱藏提示文字
            }
        }
    }

    void Update()
    {
        // 檢查玩家是否在範圍內且按下F鍵
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            PickUpKey();
        }
    }

    void PickUpKey()
    {
        // 玩家拾取鑰匙
        finalDoor.hasKey = true;

        // 鑰匙消失
        Destroy(gameObject);

        // 隱藏提示文字
        if (pickupText != null)
        {
            pickupText.enabled = false;
        }

        // 通知怪物開始追擊
        EnemyAIPatrol[] enemies = FindObjectsOfType<EnemyAIPatrol>();
        foreach (EnemyAIPatrol enemy in enemies)
        {
            enemy.StartChasing();
        }
    }
}