using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Door : MonoBehaviour
{
    public Animator door;
    public DialogManager dialogManager; // 對話框管理器
    public string requiredKey; // 此門所需的鑰匙
    public string doorName; // 此門的名稱（用於顯示互動訊息）
    public AudioSource doorAudio; // 門的音源
    public AudioClip openSound; // 開門音效
    public AudioClip closeSound; // 關門音效

    private bool inPlayer;
    private bool isOpen;

    void Start()
    {
        inPlayer = false;
        isOpen = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inPlayer = true;
            ShowInteractionMessage(); // 顯示互動訊息
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inPlayer = false;
            HideInteractionMessage(); // 隱藏互動訊息
        }
    }

    void Update()
    {
        if (inPlayer && Input.GetButtonDown("Interact"))
        {
            if (!isOpen && GameManager.Instance.HasKey(requiredKey))
            {
                DoorOpens(); // 開啟門
            }
            else if (!isOpen)
            {
                ShowNeedKeyMessage(); // 顯示需要鑰匙的訊息
            }
            else
            {
                DoorCloses(); // 關閉門
            }
        }
    }

    // 開啟門
    void DoorOpens()
    {
        door.SetBool("open", true);
        door.SetBool("closed", false);
        isOpen = true;
        if (doorAudio && openSound)
        {
            doorAudio.clip = openSound;
            doorAudio.Play();
        }
    }

    // 關閉門
    void DoorCloses()
    {
        door.SetBool("open", false);
        door.SetBool("closed", true);
        isOpen = false;
        if (doorAudio && closeSound)
        {
            doorAudio.clip = closeSound;
            doorAudio.Play();
        }
    }

    // 顯示互動訊息
    void ShowInteractionMessage()
    {
        dialogManager.ShowDialog("按下 'E' 開啟 " + doorName);
    }

    // 隱藏互動訊息
    void HideInteractionMessage()
    {
        dialogManager.HideDialog();
    }

    // 顯示需要鑰匙的訊息
    void ShowNeedKeyMessage()
    {
        dialogManager.ShowDialog("鎖住了，需要 " + requiredKey + " 開啟");
    }
}
