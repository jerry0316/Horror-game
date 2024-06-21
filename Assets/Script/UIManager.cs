using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance; // 單例

    public Text keyInfoText; // 顯示鑰匙信息的UI Text

    private void Awake()
    {
        // 單例模式，確保只有一個UIManager實例存在
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
        // 顯示需要哪個房間的鑰匙信息
        keyInfoText.text = "需要 " + doorName + " 的鑰匙";
    }
}
