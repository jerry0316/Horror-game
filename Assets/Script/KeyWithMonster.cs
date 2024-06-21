using UnityEngine;

public class KeyWithMonster : MonoBehaviour
{
    bool playerInRange = false;
    
    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            // 玩家按下 F 鍵，撿起鑰匙
            PickUpKey();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 玩家靠近鑰匙，顯示提示
            playerInRange = true;
            Debug.Log("Press 'F' to pick up the key.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 玩家離開鑰匙範圍，隱藏提示
            playerInRange = false;
        }
    }

    // 撿起鑰匙的方法
    void PickUpKey()
    {
        // 銷毀鑰匙
        Destroy(gameObject);
        

        // 通知怪物開始追擊
        EnemyAIPatrol[] enemies = FindObjectsOfType<EnemyAIPatrol>();
        foreach (EnemyAIPatrol enemy in enemies)
        {
            enemy.StartChasing();
        }
    }
}
