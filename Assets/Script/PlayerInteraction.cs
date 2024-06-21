using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NavKeypad
{
    public class PlayerInteraction : MonoBehaviour
    {
        public Keypad keypad; // 密碼鎖腳本的引用

        private bool nearKeypad = false;

        void Update()
        {
            if (nearKeypad && Input.GetKeyDown(KeyCode.E))
            {
                // 检查玩家是否靠近密碼鎖并按下了 "E" 键
                ShowPasswordInput();
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                nearKeypad = true;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                nearKeypad = false;
            }
        }

        void ShowPasswordInput()
        {
            // 调用密碼鎖腳本中的方法，显示密码输入界面
            keypad.gameObject.SetActive(true);
        }
    }
}
