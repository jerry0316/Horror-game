using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public TMPro.TextMeshProUGUI dialogText; // �Ω���ܹ�ܪ� UI �奻����

    // ��ܹ�ܮءA�ó]�m�������奻
    public void ShowDialog(string text)
    {
        dialogText.text = text;
        dialogText.gameObject.SetActive(true);
    }

    // ���ù�ܮ�
    public void HideDialog()
    {
        dialogText.gameObject.SetActive(false);
    }
}
