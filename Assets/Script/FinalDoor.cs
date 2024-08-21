using UnityEngine;
using UnityEngine.Video;
using TMPro;

public class FinalDoor : MonoBehaviour
{
    public VideoPlayer videoPlayer;  // 這裡引用VideoPlayer物件
    public bool hasKey = false;  // 用於檢查玩家是否擁有鑰匙
    private bool isPlayerInRange = false;  // 檢查玩家是否在範圍內
    public TextMeshProUGUI doorHintText;  // 引用提示文本的TextMeshProUGUI元件

    void Start()
    {
        // 確保影片不會在遊戲開始時自動播放
        videoPlayer.Stop();  // 停止影片以防自動播放
        Debug.Log("Start() 初始化：影片已停止播放");

        // 確保提示文本在開始時隱藏
        if (doorHintText != null)
        {
            doorHintText.enabled = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;  // 玩家進入範圍
            if (doorHintText != null)
            {
                doorHintText.text = "按下E逃離咒院";  // 顯示逃離提示
                doorHintText.enabled = true;  // 顯示提示文本
            }
            Debug.Log("玩家進入範圍，顯示提示。");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;  // 玩家離開範圍
            if (doorHintText != null)
            {
                doorHintText.enabled = false;  // 隱藏提示文本
            }
            Debug.Log("玩家離開範圍，隱藏提示。");
        }
    }

    void Update()
    {
        // 檢查玩家是否在範圍內且按下E鍵
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (hasKey)
            {
                PlayEndingVideo();
            }
            else
            {
                if (doorHintText != null)
                {
                    doorHintText.text = "你需要找到逃離咒院的最終鑰匙!";  // 顯示需要鑰匙的提示
                }
                Debug.Log("你需要一把鑰匙來打開門。");
            }
        }
    }

    void PlayEndingVideo()
    {
        // 播放結尾影片
        videoPlayer.Play();
        Debug.Log("播放結尾影片");

        // 隱藏提示文本
        if (doorHintText != null)
        {
            doorHintText.enabled = false;
        }
    }
}
