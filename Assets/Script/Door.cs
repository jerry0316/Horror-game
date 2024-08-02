using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Door : MonoBehaviour
{
    public Animator door;
    public DialogManager dialogManager; // ��ܮغ޲z��
    public string requiredKey; // �����һݪ��_��
    public string doorName; // �������W�١]�Ω���ܤ��ʰT���^
    public AudioSource doorAudio; // ��������
    public AudioClip openSound; // �}������
    public AudioClip closeSound; // ��������

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
            ShowInteractionMessage(); // ��ܤ��ʰT��
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inPlayer = false;
            HideInteractionMessage(); // ���ä��ʰT��
        }
    }

    void Update()
    {
        if (inPlayer && Input.GetButtonDown("Interact"))
        {
            if (!isOpen && GameManager.Instance.HasKey(requiredKey))
            {
                DoorOpens(); // �}�Ҫ�
            }
            else if (!isOpen)
            {
                ShowNeedKeyMessage(); // ��ܻݭn�_�ͪ��T��
            }
            else
            {
                DoorCloses(); // ������
            }
        }
    }

    // �}�Ҫ�
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

    // ������
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

    // ��ܤ��ʰT��
    void ShowInteractionMessage()
    {
        dialogManager.ShowDialog("按下E開啟" + doorName);
    }

    // ���ä��ʰT��
    void HideInteractionMessage()
    {
        dialogManager.HideDialog();
    }

    // ��ܻݭn�_�ͪ��T��
    void ShowNeedKeyMessage()
    {
        dialogManager.ShowDialog("需要" + requiredKey + "開啟");
    }
}
