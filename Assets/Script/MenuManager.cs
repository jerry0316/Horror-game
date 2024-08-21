using UnityEngine;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    void Awake()
    {
        // �bAwake���q�ˬd�çR���h�l��Event System
        EventSystem[] eventSystems = FindObjectsOfType<EventSystem>();
        if (eventSystems.Length > 1)
        {
            for (int i = 1; i < eventSystems.Length; i++)
            {
                DestroyImmediate(eventSystems[i].gameObject);  // �ߧY�R���h�l��Event System
            }
        }
    }

    void Start()
    {
        // �bStart���q�A���ˬd�H�T�O���|���h�l��Event System�s�b
        CheckAndRemoveExtraEventSystems();
    }

    void CheckAndRemoveExtraEventSystems()
    {
        EventSystem[] eventSystems = FindObjectsOfType<EventSystem>();
        if (eventSystems.Length > 1)
        {
            for (int i = 1; i < eventSystems.Length; i++)
            {
                Destroy(eventSystems[i].gameObject);  // �P���h�l��Event System
            }
        }
    }
}
