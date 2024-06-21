using UnityEngine;
using UnityEngine.UI;

namespace KeypadSystem
{
    public class KPUIManager : MonoBehaviour
    {
        [Header("Crosshair")]
        [SerializeField] private Image crosshair = null;

        [Header("UI Prompt")]
        [SerializeField] private GameObject interactPrompt = null;

        [Header("Keypad Type Input Fields")]
        [SerializeField] private InputField modernCodeText = null;
        [SerializeField] private InputField scifiCodeText = null;
        [SerializeField] private InputField keyboardCodeText = null;
        [SerializeField] private InputField bombCodeText = null;

        [Header("Phone Type Canvas Fields")]
        [SerializeField] private GameObject modernCanvas = null;
        [SerializeField] private GameObject scifiCanvas = null;
        [SerializeField] private GameObject keyboardCanvas = null;
        [SerializeField] private GameObject bombCanvas = null;

        private bool firstKeypadClick;
        private KeypadController _keypadController;
        private KeypadType _keypadType;
        private enum KeypadType { None, Modern, Scifi, Keyboard, Bomb };

        public static KPUIManager instance;

        void Awake()
        {
            if (instance != null) { Destroy(gameObject); }
            else { instance = this; DontDestroyOnLoad(gameObject); }
        }

        public void SetKeypadController(KeypadController _myController)
        {
            _keypadController = _myController;
        }

        public void ShowModernCanvas(bool on)
        {
            modernCanvas.SetActive(on);
            _keypadType = KeypadType.Modern;
        }

        public void ShowScifiCanvas(bool on)
        {
            scifiCanvas.SetActive(on);
            _keypadType = KeypadType.Scifi;
        }

        public void ShowKeyboardCanvas(bool on)
        {
            keyboardCanvas.SetActive(on);
            _keypadType = KeypadType.Keyboard;
        }

        public void ShowBombCanvas(bool on)
        {
            bombCanvas.SetActive(on);
            _keypadType = KeypadType.Bomb;
        }

        public void KeyPressString(string keyString)
        {
            _keypadController.SingleBeepSound();

            if (!firstKeypadClick)
            {
                ClearKeypadInputFields();
                firstKeypadClick = true;
            }

            InputField activeInputField = GetActiveKeypadInputField();
            if (activeInputField != null && activeInputField.characterLimit <= (_keypadController.inputLimit - 1))
            {
                activeInputField.characterLimit++;
                activeInputField.text += keyString;
            }
        }

        public void KeyPressEnter()
        {
            _keypadController.SingleBeepSound();
            InputField activeInputField = GetActiveKeypadInputField();
            if (activeInputField != null)
            {
                _keypadController.CheckCode(activeInputField);
            }
        }

        public void KeyPressClear(bool playSound)
        {
            if (playSound)
            {
                _keypadController.SingleBeepSound();
            }
            InputField activeInputField = GetActiveKeypadInputField();
            ClearKeypadFieldData(activeInputField);
        }

        public void KeyPressClose()
        {
            KeyPressClear(false);
            _keypadController.CloseKeypad();
        }

        private void ClearKeypadInputFields()
        {
            ClearKeypadFieldData(modernCodeText);
            ClearKeypadFieldData(scifiCodeText);
            ClearKeypadFieldData(keyboardCodeText);
            ClearKeypadFieldData(bombCodeText);
        }

        private void ClearKeypadFieldData(InputField inputField)
        {
            if (inputField != null)
            {
                inputField.characterLimit = 0;
                inputField.text = string.Empty;
            }
        }

        private InputField GetActiveKeypadInputField()
        {
            switch (_keypadType)
            {
                case KeypadType.Modern:
                    return modernCodeText;
                case KeypadType.Scifi:
                    return scifiCodeText;
                case KeypadType.Keyboard:
                    return keyboardCodeText;
                case KeypadType.Bomb:
                    return bombCodeText;
                default:
                    return null;
            }
        }

        public void HighlightCrosshair(bool on)
        {
            if (on)
            {
                crosshair.color = Color.red;
            }
            else
            {
                crosshair.color = Color.white;
            }
        }

        public void ShowInteractPrompt(bool on)
        {
            interactPrompt.SetActive(on);
        }

        public void ShowCrosshair(bool on)
        {
            crosshair.enabled = !on;
            if (on)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;  
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
}
