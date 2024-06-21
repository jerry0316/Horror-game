using UnityEngine;
using TMPro;

public class Key : MonoBehaviour
{
    public string keyName; // 鑰匙的名稱
    public TextMeshProUGUI pickUpText; // 用於顯示撿取鑰匙的訊息的 UI 文本元素
    public AudioSource pickUpSound; // 撿拾鑰匙的音效

    private bool canPickUp = false; // 是否可以撿取鑰匙

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canPickUp = true; // 進入撿取範圍
            ShowPickUpMessage(); // 顯示撿取鑰匙的訊息
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canPickUp = false; // 離開撿取範圍
            HidePickUpMessage(); // 隱藏撿取鑰匙的訊息
        }
    }

    private void Update()
    {
        if (canPickUp && Input.GetKeyDown(KeyCode.F))
        {
            PickUpKey(); // 當按下 F 鍵且在撿取範圍內時，撿取鑰匙
        }
    }

    void PickUpKey()
    {
        // 播放撿拾鑰匙的音效
        if (pickUpSound != null)
        {
            pickUpSound.Play();
        }

        // 將鑰匙添加到 GameManager 的鑰匙列表中
        GameManager.Instance.AddKey(keyName);
        // 顯示鑰匙對應的門的訊息
        Debug.Log("撿起 " + keyName);
        Destroy(gameObject); // 銷毀鑰匙物件
    }

    void ShowPickUpMessage()
    {
        // 顯示撿取鑰匙的訊息
        pickUpText.gameObject.SetActive(true);
        pickUpText.text = "按下 'F' 撿起 " + keyName;
    }

    void HidePickUpMessage()
    {
        // 隱藏撿取鑰匙的訊息
        pickUpText.gameObject.SetActive(false);
    }
}
