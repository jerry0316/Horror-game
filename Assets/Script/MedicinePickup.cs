using TMPro;
using UnityEngine;
using UnityEngine.AI; // 確保引入 NavMesh 命名空間

public class MedicinePickup : MonoBehaviour
{
    public int sanityRestoreAmount = 20; // 恢復的理智值量
    private SanityManager sanityManager; // 參考到理智管理器
    private bool isCollected = false; // 用於判斷藥物是否已被撿起

    public TextMeshProUGUI MedicinePickupText; // 用於顯示提示的文本

    private void Start()
    {
        sanityManager = FindObjectOfType<SanityManager>();
        if (MedicinePickupText != null)
        {
            MedicinePickupText.gameObject.SetActive(false); // 初始化時隱藏提示文本
        }
        else
        {
            Debug.LogError("MedicinePickupText 未正確設置！請確保它已在 Inspector 中連結。");
        }
    }

    private void Update()
    {
        // 檢查玩家按下 'E' 鍵並且藥物未被撿起
        if (!isCollected && Input.GetKeyDown(KeyCode.E))
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 2f); // 使用圓形範圍檢測玩家是否在附近
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Player"))
                {
                    CollectMedicine(); // 撿起藥物
                    break;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MedicinePickupText.text = "按 E 撿起藥物"; // 顯示提示文本
            MedicinePickupText.gameObject.SetActive(true); // 顯示提示
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MedicinePickupText.gameObject.SetActive(false); // 隱藏提示
        }
    }

    private void CollectMedicine()
    {
        if (sanityManager != null)
        {
            sanityManager.IncreaseSanity(sanityRestoreAmount); // 恢復理智值
        }

        // 隱藏提示文本
        if (MedicinePickupText != null)
        {
            MedicinePickupText.gameObject.SetActive(false); // 隱藏提示文本
        }

        isCollected = true; // 設置為已收集狀態
        gameObject.SetActive(false); // 隱藏藥物物件，避免多次撿起

        // 可選：您可以在這裡觸發一個回收或重新生成的事件
        Invoke("Respawn", 3f); // 30秒後重新生成藥物
    }


    private void Respawn()
    {
        // 隨機選擇一個新的位置來生成藥物
        Vector3 newPosition = GetRandomNavMeshPosition();
        if (newPosition != Vector3.zero) // 確保生成位置有效
        {
            transform.position = newPosition;
            gameObject.SetActive(true); // 顯示藥物物件
            isCollected = false; // 重置收集狀態
        }
    }

    private Vector3 GetRandomNavMeshPosition()
    {
        Vector3 randomPosition = Random.insideUnitSphere * 10; // 隨機位置範圍
        randomPosition.y = 0; // 調整 y 軸以符合地面

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPosition, out hit, 10f, NavMesh.AllAreas))
        {
            return hit.position; // 返回在導航網格上的隨機位置
        }

        return Vector3.zero; // 若沒有可用位置，返回零向量
    }
}
