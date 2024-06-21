using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public TMPro.TextMeshProUGUI dialogText; // 用於顯示對話的 UI 文本元素

    // 顯示對話框，並設置相應的文本
    public void ShowDialog(string text)
    {
        dialogText.text = text;
        dialogText.gameObject.SetActive(true);
    }

    // 隱藏對話框
    public void HideDialog()
    {
        dialogText.gameObject.SetActive(false);
    }
}
