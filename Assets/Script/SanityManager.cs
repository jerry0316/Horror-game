using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Video;

public class SanityManager : MonoBehaviour
{
    public int sanity = 100; // �z����
    public TextMeshProUGUI sanityText; // �Ω���ܲz���Ȫ� UI �奻
    public GameObject staticEffectObject; // �Ω�����R�q�ĪG������
    private Coroutine staticEffectCoroutine; // �Ω󱱨��R�q�ĪG����{
    private VideoPlayer staticVideoPlayer; // VideoPlayer �ե�

    private void Start()
    {
        StartCoroutine(DecreaseSanityOverTime());
        UpdateSanityUI();
        staticEffectObject.SetActive(false); // ��l�Ʈ������R�q�ĪG����

        // ��� VideoPlayer �ե�
        staticVideoPlayer = staticEffectObject.GetComponent<VideoPlayer>();
        if (staticVideoPlayer != null)
        {
            staticVideoPlayer.loopPointReached += OnVideoFinished;
        }
        else
        {
            Debug.LogError("�R�q�ĪG���󥼪��[ VideoPlayer �ե�I");
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
            staticVideoPlayer.Play(); // �����R�q���W
            Debug.Log("Playing static video.");

            // �Ұ��R�q�ĪG��{
            if (staticEffectCoroutine == null)
            {
                staticEffectCoroutine = StartCoroutine(StaticEffectCoroutine());
            }
        }
        else
        {
            Debug.LogError("�R�q�ĪG���󥼳]�m�Τw�Q�T�ΡI");
        }
    }

    private IEnumerator StaticEffectCoroutine()
    {
        while (sanity <= 0) // ��z���Ȥp�󵥩�0�ɫ������
        {
            staticVideoPlayer.Play(); // �����R�q���W
            yield return new WaitForSeconds(20f); // �C5����@���R�q�ĪG
        }

        StopStaticEffect(); // ��z���Ȥj��0�ɰ����R�q�ĪG
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        // �o�̥i�H��ܬO�_�b���W���񵲧������Y�Ǿާ@
    }

    private void StopStaticEffect()
    {
        if (staticEffectObject != null)
        {
            staticEffectObject.SetActive(false);
            staticVideoPlayer.Stop(); // �����R�q���W����
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
        DecreaseSanity(10); // �C��Ĳ�o�R�q�ĪG�ɡA����10�I�z����
    }
}
