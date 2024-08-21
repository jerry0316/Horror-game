using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public class FinalDoor : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public bool hasKey = false;
    private bool isPlayerInRange = false;
    public TextMeshProUGUI doorHintText;

    void Start()
    {
        videoPlayer.Stop();
        videoPlayer.loopPointReached += OnVideoEnd;

        if (doorHintText != null)
        {
            doorHintText.enabled = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            if (doorHintText != null)
            {
                doorHintText.text = "按下E逃離咒院";
                doorHintText.enabled = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (doorHintText != null)
            {
                doorHintText.enabled = false;
            }
        }
    }

    void Update()
    {
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
                    doorHintText.text = "你需要找到逃離咒院的最終鑰匙!";
                }
            }
        }
    }

    void PlayEndingVideo()
    {
        // 在播放影片之前刪除當前場景的Event System
        DestroyEventSystems();

        videoPlayer.Play();
        if (doorHintText != null)
        {
            doorHintText.enabled = false;
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    void DestroyEventSystems()
    {
        EventSystem[] eventSystems = FindObjectsOfType<EventSystem>();
        foreach (var es in eventSystems)
        {
            Destroy(es.gameObject);
        }
    }
}
