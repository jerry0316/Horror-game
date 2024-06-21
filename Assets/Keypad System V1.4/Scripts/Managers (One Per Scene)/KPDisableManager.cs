using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

namespace KeypadSystem
{
    public class KPDisableManager : MonoBehaviour
    {
        [SerializeField] private FirstPersonController player = null;
        [SerializeField] private KeypadInteractor keypadInteractor = null;

        public static KPDisableManager instance;

        void Awake()
        {
            if (instance != null) { Destroy(gameObject); }
            else { instance = this; DontDestroyOnLoad(gameObject); }
        }

        public void DisablePlayer(bool disable)
        {
            if(player != null) 
            {
                player.enabled = !disable;
            }
            else
            {
                Debug.LogError("You may want to add the player object into the inspector slot of the Disable Manager - Or change this if you're using a different controller");
            }

            if (keypadInteractor != null)
            {
                keypadInteractor.enabled = !disable;
            }
            else
            {
                Debug.LogError("Add the keypad interactor script (Usually found on the Main Camera) to the Disable Manager");
            }
            KPUIManager.instance.ShowCrosshair(disable);
        }
    }
}
