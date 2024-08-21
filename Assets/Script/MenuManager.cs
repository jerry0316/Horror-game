using UnityEngine;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    void Awake()
    {
        // 在Awake階段檢查並刪除多餘的Event System
        EventSystem[] eventSystems = FindObjectsOfType<EventSystem>();
        if (eventSystems.Length > 1)
        {
            for (int i = 1; i < eventSystems.Length; i++)
            {
                DestroyImmediate(eventSystems[i].gameObject);  // 立即刪除多餘的Event System
            }
        }
    }

    void Start()
    {
        // 在Start階段再次檢查以確保不會有多餘的Event System存在
        CheckAndRemoveExtraEventSystems();
    }

    void CheckAndRemoveExtraEventSystems()
    {
        EventSystem[] eventSystems = FindObjectsOfType<EventSystem>();
        if (eventSystems.Length > 1)
        {
            for (int i = 1; i < eventSystems.Length; i++)
            {
                Destroy(eventSystems[i].gameObject);  // 銷毀多餘的Event System
            }
        }
    }
}
