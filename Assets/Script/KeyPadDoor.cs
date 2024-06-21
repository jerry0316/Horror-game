using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NavKeypad
{
    public class KeyPadDoor : MonoBehaviour
    {
        public Keypad keypad; // 密碼鎖腳本的引用
        public Animator door;
        public DialogManager dialogManager; // 對話框管理器
        public string doorName; // 此門的名稱（用於顯示互動訊息）

        private bool inPlayer;
        private bool isOpen;

        void Start()
        {
            inPlayer = false;
            isOpen = false;

            // 添加密碼鎖開門事件的監聽
            keypad.OnAccessGranted.AddListener(DoorOpens);
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
                // 检查门是否已经打开
                if (!isOpen)
                {
                    // 显示输入密码的界面
                    ShowPasswordInput();
                }
            }
        }

        void ShowPasswordInput()
        {
            // 在对话框中显示输入密码的提示信息
            dialogManager.ShowDialog("Enter the password to open " + doorName);

            // 激活密碼鎖对象，以便玩家可以输入密码
            keypad.gameObject.SetActive(true);

        }

        // 開啟門
        void DoorOpens()
        {
            door.SetBool("open", true);
            door.SetBool("closed", false);
            isOpen = true;
        }

        // 顯示互動訊息
        void ShowInteractionMessage()
        {
            dialogManager.ShowDialog("Press 'E' to open " + doorName);
        }

        // 隱藏互動訊息
        void HideInteractionMessage()
        {
            dialogManager.HideDialog();
        }
    }
}
