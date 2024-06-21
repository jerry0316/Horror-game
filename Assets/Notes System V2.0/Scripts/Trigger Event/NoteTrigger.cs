using UnityEngine;

namespace NoteSystem
{
    public class NoteTrigger : MonoBehaviour
    {
        [Header("Note SO")]
        [SerializeField] private Note noteData = null;

        [Header("Tag that is used for detection")]
        [SerializeField] private string playerTag = "Player";

        private bool canUse;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(playerTag))
            {
                canUse = true;
                NoteUIManager.instance.ShowInteractPrompt(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(playerTag))
            {
                canUse = false;
                NoteUIManager.instance.ShowInteractPrompt(false);
            }
        }

        private void Update()
        {
            if (canUse)
            {
                if (Input.GetKeyDown(NoteInputManager.instance.triggerInteractKey))
                {
                    NoteUIManager.instance.ShowInteractPrompt(false);
                    ShowNote();
                }
            }
        }

        public void ShowNote()
        {
            NoteController.instance.GetTriggerObject(gameObject);
            NoteController.instance.CurrentNoteSource = NoteController.NoteSource.World;
            NoteController.instance.ShowNote(noteData);
        }
    }
}
