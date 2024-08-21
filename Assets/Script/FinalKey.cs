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
            Debug.Log("Start method called. Pickup Text is initialized and hidden.");
        }
        else
        {
            Debug.LogError("Pickup Text is not assigned in the inspector!");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;  // 玩家進入範圍
            if (pickupText != null)
            {
                pickupText.text = "按下F撿起鑰匙";  // 顯示提示文字
                pickupText.enabled = true;  // 顯示Text元件
                Debug.Log("Player entered the trigger area. Text displayed.");
                Debug.Log("Text enabled: " + pickupText.enabled);  // 確認Text已啟用
            }
            else
            {
                Debug.LogError("Pickup Text is missing when trying to display text.");
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
                Debug.Log("Player exited the trigger area. Text hidden.");
            }
            else
            {
                Debug.LogError("Pickup Text is missing when trying to hide text.");
            }
        }
    }

    void Update()
    {
        Debug.Log("Update: isPlayerInRange = " + isPlayerInRange);
        Debug.Log("Update: Pickup Text enabled = " + pickupText.enabled);  // 追蹤Text狀態
        
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
        Debug.Log("You picked up the key! hasKey is now: " + finalDoor.hasKey);

        // 鑰匙消失
        Destroy(gameObject);

        // 隱藏提示文字
        if (pickupText != null)
        {
            pickupText.enabled = false;
            Debug.Log("Pickup Text hidden after picking up the key.");
        }

        // 通知怪物開始追擊
        EnemyAIPatrol[] enemies = FindObjectsOfType<EnemyAIPatrol>();
        foreach (EnemyAIPatrol enemy in enemies)
        {
            enemy.StartChasing();
            Debug.Log("Enemy " + enemy.name + " started chasing.");
        }
    }
}