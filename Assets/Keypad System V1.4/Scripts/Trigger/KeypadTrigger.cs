using UnityEngine;

namespace KeypadSystem
{
    public class KeypadTrigger : MonoBehaviour
    {
        [Space(10)]
        [SerializeField] private KeypadItem keypadObject = null;

        [Space(10)]
        [SerializeField] private string playerTag = "Player";

        private bool canUse;

        private void Update()
        {
            ShowKeypadInput();
        }

        void ShowKeypadInput()
        {
            if (canUse && Input.GetKeyDown(KPInputManager.instance.triggerInteractKey))
            {
                keypadObject.ShowKeypad();
                KPUIManager.instance.ShowInteractPrompt(false);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(playerTag))
            {
                canUse = true;
                KPUIManager.instance.ShowInteractPrompt(canUse);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(playerTag))
            {
                canUse = false;
                KPUIManager.instance.ShowInteractPrompt(canUse);
            }
        }
    }
}
