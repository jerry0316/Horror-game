using UnityEngine;

namespace KeypadSystem
{
    [RequireComponent(typeof(Camera))]
    public class KeypadInteractor : MonoBehaviour
    {
        [Header("Raycast Features")]
        [SerializeField] private float interactDistance = 5;

        [Header("Raycast Keypad Tag")]
        [SerializeField] private string keypadTag = "Keypad";

        private KeypadItem keypadItem;
        private Camera _camera;

        void Start()
        {
            _camera = GetComponent<Camera>();
        }

        void Update()
        {
            if (Physics.Raycast(_camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f)), transform.forward, out RaycastHit hit, interactDistance))
            {
                var raycastedObj = hit.collider.GetComponent<KeypadItem>();
                if (raycastedObj != null && raycastedObj.CompareTag(keypadTag))
                {
                    keypadItem = raycastedObj;
                    HighlightCrosshair(true);
                }
                else
                {
                    ClearInteractable();
                } 
            }
            else
            {
                ClearInteractable();
            }

            if (keypadItem != null)
            {
                if (Input.GetKeyDown(KPInputManager.instance.interactKey))
                {
                    keypadItem.ShowKeypad();
                }
            }
        }

        private void ClearInteractable()
        {
            if (keypadItem != null)
            {
                HighlightCrosshair(false);
                keypadItem = null;
            }
        }

        void HighlightCrosshair(bool on)
        {
            KPUIManager.instance.HighlightCrosshair(on);
        }
    }
}
