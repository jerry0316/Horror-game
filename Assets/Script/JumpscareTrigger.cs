using UnityEngine;
using UnityEngine.UI; // 引入UI命名空間

public class JumpscareTrigger : MonoBehaviour
{
    public GameObject scareImage;   // 全屏的恐怖圖片 (UI Image)
    public AudioSource scareSound;  // 恐怖音效
    private bool hasTriggered = false;  // 控制只觸發一次

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasTriggered) // 檢查玩家Tag並確保只觸發一次
        {
            hasTriggered = true;  // 設置為已觸發
            scareSound.Play();  // 播放恐怖音效
            scareImage.SetActive(true);  // 顯示全屏恐怖圖片
           
            Invoke("StopScare", 2f);  // 2秒後調用 StopScare 方法
        }
    }

    // 2秒後停止圖片顯示和音效
    private void StopScare()
    {
        scareImage.SetActive(false);  // 隱藏恐怖圖片
        scareSound.Stop();  // 停止恐怖音效
    }
}
