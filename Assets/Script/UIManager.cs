using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance; // ���

    public Text keyInfoText; // ����_�ͫH����UI Text

    private void Awake()
    {
        // ��ҼҦ��A�T�O�u���@��UIManager��Ҧs�b
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void DisplayKeyInfo(string doorName)
    {
        // ��ܻݭn���өж����_�ͫH��
        keyInfoText.text = "�ݭn " + doorName + " ���_��";
    }
}
