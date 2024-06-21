using UnityEngine;

namespace KeypadSystem
{
    public class KeypadItem : MonoBehaviour
    {
        [Space(10)]
        [SerializeField] private KeypadController _keypadController = null;

        public void ShowKeypad()
        {
            _keypadController.ShowKeypad();
        }
    }
}
