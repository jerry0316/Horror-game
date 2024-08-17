using UnityEngine;

public class FinalKey : MonoBehaviour
{
    public FinalDoor finalDoor;  // 引用FinalDoor腳本
    private bool isPlayerInRange = false;  // 檢查玩家是否在範圍內

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;  // 玩家進入範圍
            Debug.Log("按F撿起鑰匙");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;  // 玩家離開範圍
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
        Debug.Log("你已獲得鑰匙！");
    }
}
