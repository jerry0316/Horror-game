using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Video;

public class SanityManager : MonoBehaviour
{
    public int sanity = 100; // 理智值
    public TextMeshProUGUI sanityText; // 用於顯示理智值的 UI 文本
    public GameObject staticEffectObject; // 用於顯示靜電效果的物件
    private Coroutine staticEffectCoroutine; // 用於控制靜電效果的協程
    private VideoPlayer staticVideoPlayer; // VideoPlayer 組件

    private void Start()
    {
        StartCoroutine(DecreaseSanityOverTime());
        UpdateSanityUI();
        staticEffectObject.SetActive(false); // 初始化時隱藏靜電效果物件

        // 獲取 VideoPlayer 組件
        staticVideoPlayer = staticEffectObject.GetComponent<VideoPlayer>();
        if (staticVideoPlayer != null)
        {
            staticVideoPlayer.loopPointReached += OnVideoFinished;
        }
        else
        {
            Debug.LogError("靜電效果物件未附加 VideoPlayer 組件！");
        }
    }

    private IEnumerator DecreaseSanityOverTime()
    {
        while (sanity > 0)
        {
            yield return new WaitForSeconds(10);
            sanity -= 1;
            Debug.Log("Decreasing sanity. Current Sanity: " + sanity);
            UpdateSanityUI();

            if (sanity <= 0)
            {
                Debug.Log("Sanity reached zero! Triggering static effect.");
                TriggerStaticEffect();
            }
        }
    }

    private void UpdateSanityUI()
    {
        sanityText.text = "Sanity: " + sanity;
    }

    public void DecreaseSanity(int amount)
    {
        sanity -= amount;
        Debug.Log("Decreasing sanity by " + amount + ". Current Sanity: " + sanity);
        UpdateSanityUI();

        if (sanity <= 0)
        {
            Debug.Log("Sanity reached zero! Triggering static effect.");
            TriggerStaticEffect();
        }
    }

    public void IncreaseSanity(int amount)
    {
        sanity = Mathf.Min(sanity + amount, 100);
        Debug.Log("Increasing sanity by " + amount + ". Current Sanity: " + sanity);
        UpdateSanityUI();

        if (sanity > 0)
        {
            StopStaticEffect();
        }
    }

    private void TriggerStaticEffect()
    {
        Debug.Log("Static effect triggered.");

        if (staticEffectObject != null)
        {
            staticEffectObject.SetActive(true);
            staticVideoPlayer.Play(); // 播放靜電視頻
            Debug.Log("Playing static video.");

            // 啟動靜電效果協程
            if (staticEffectCoroutine == null)
            {
                staticEffectCoroutine = StartCoroutine(StaticEffectCoroutine());
            }
        }
        else
        {
            Debug.LogError("靜電效果物件未設置或已被禁用！");
        }
    }

    private IEnumerator StaticEffectCoroutine()
    {
        while (sanity <= 0) // 當理智值小於等於0時持續執行
        {
            staticVideoPlayer.Play(); // 播放靜電視頻
            yield return new WaitForSeconds(20f); // 每5秒播放一次靜電效果
        }

        StopStaticEffect(); // 當理智值大於0時停止靜電效果
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        // 這裡可以選擇是否在視頻播放結束後執行某些操作
    }

    private void StopStaticEffect()
    {
        if (staticEffectObject != null)
        {
            staticEffectObject.SetActive(false);
            staticVideoPlayer.Stop(); // 停止靜電視頻播放
            Debug.Log("Stopped static video.");
        }

        if (staticEffectCoroutine != null)
        {
            StopCoroutine(staticEffectCoroutine);
            staticEffectCoroutine = null;
        }
    }

    public void DecreaseSanityForStaticEffect()
    {
        DecreaseSanity(10); // 每次觸發靜電效果時，扣除10點理智值
    }
}
