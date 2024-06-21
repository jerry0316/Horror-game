using UnityEngine;

namespace KeypadSystem
{
    public class DoorController : MonoBehaviour
    {
        [Space(10)]
        [SerializeField] private string myAnimation = "OpenDoor";

        private Animator doorAnim;

        private void Awake()
        {
            doorAnim = gameObject.GetComponent<Animator>();
        }

        public void PlayAnimation()
        {
            doorAnim.Play(myAnimation, 0, 0.0f);
        }
    }
}
