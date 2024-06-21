using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Events;

namespace KeypadSystem
{
    [System.Serializable]
    public class KeypadCodes
    {
        public string keypadCode;
        [Space(10)]
        public UnityEvent keypadEvent;
    }

    public class KeypadController : MonoBehaviour
    {
        [Space(10)]
        [SerializeField] private KeypadType _keypadType = KeypadType.None;
        private enum KeypadType { None, Modern, Scifi, Keyboard, Bomb };

        [Space(10)]
        [SerializeField] private int _inputLimit = 10;

        [Space(10)]
        [SerializeField] private KeypadCodes[] keypadCodesList = null;

        [Header("Keypad Interaction Sounds")]
        [SerializeField] private Sound keypadOpenSound = null;
        [SerializeField] private Sound keypadCloseSound = null;

        [Header("Keypad Input Sounds")]
        [SerializeField] private Sound keypadBeepSound = null;
        [SerializeField] private Sound keypadDeniedSound = null;
        [SerializeField] private Sound keypadApproveSound = null;

        [Header("Trigger Event")]
        [SerializeField] bool isTriggerEvent = false;
        [SerializeField] private KeypadTrigger triggerObject = null;

        public int inputLimit
        {
            get { return _inputLimit; }
            set { _inputLimit = value; }
        }

        private bool isOpen = false;

        private void Update()
        {
            if (isOpen && Input.GetKeyDown(KPInputManager.instance.closeKey))
            {
                CloseKeypad();
            }
        }

        public void ShowKeypad()
        {
            isOpen = true;
            KPDisableManager.instance.DisablePlayer(true);
            KPUIManager.instance.SetKeypadController(this);
            SetKeypadTypeActive(true);
            OpenSound();

            if (isTriggerEvent)
            {
                KPUIManager.instance.ShowInteractPrompt(false);
                triggerObject.enabled = false;
            }
        }

        public void CloseKeypad()
        {
            isOpen = false;
            KPDisableManager.instance.DisablePlayer(false);
            KPUIManager.instance.KeyPressClear(false);
            CloseSound();
            SetKeypadTypeActive(false);

            if (isTriggerEvent)
            {
                KPUIManager.instance.ShowInteractPrompt(true);
                triggerObject.enabled = true;
            }
        }

        void SetKeypadTypeActive(bool on)
        {
            switch (_keypadType)
            {
                case KeypadType.Modern:
                    KPUIManager.instance.ShowModernCanvas(on);
                    break;
                case KeypadType.Scifi:
                    KPUIManager.instance.ShowScifiCanvas(on);
                    break;
                case KeypadType.Keyboard:
                    KPUIManager.instance.ShowKeyboardCanvas(on);
                    break;
                case KeypadType.Bomb:
                    KPUIManager.instance.ShowBombCanvas(on);
                    break;
            }
        }

        public void CheckCode(InputField numberInputField)
        {
            var code = keypadCodesList.FirstOrDefault(x => x.keypadCode == numberInputField.text);
            if (code != null)
            {
                ApproveSound();
                code.keypadEvent.Invoke();
            }
            else
            {
                DeniedSound();
            }
        }

        public void SingleBeepSound()
        {
            KPAudioManager.instance.Play(keypadBeepSound);
        }

        public void DeniedSound()
        {
            KPAudioManager.instance.Play(keypadDeniedSound);

        }

        void CloseSound()
        {
            KPAudioManager.instance.Play(keypadCloseSound);
        }

        void OpenSound()
        {
            if (keypadOpenSound != null)
            {
                KPAudioManager.instance.Play(keypadOpenSound);
            }
        }

        void ApproveSound()
        {
            if (keypadApproveSound != null)
            {
                KPAudioManager.instance.Play(keypadApproveSound);
            }
        }
    }
}
