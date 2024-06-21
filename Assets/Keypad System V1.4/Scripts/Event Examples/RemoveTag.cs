using UnityEngine;

namespace KeypadSystem
{
    public class RemoveTag : MonoBehaviour
    {
        [Space(10)]
        [SerializeField] private GameObject taggedObject = null;

        public void RemoveTags()
        {
            taggedObject.tag = "Untagged";
        }
    }
}
