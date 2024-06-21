using NoteSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoteSystem
{
    public class NoteInventory : MonoBehaviour
    {
        [SerializeField] private List<Note> notes = new List<Note>();

        public List<Note> Notes
        {
            get { return notes; }
        }

        public static NoteInventory instance;

        private void Awake()
        {
            if (instance == null) { instance = this; DontDestroyOnLoad(gameObject); }
            else { Destroy(gameObject); }
        }

        public void AddNote(Note note)
        {
            // Check if the note isn't already in the inventory
            if (!notes.Contains(note))
            {
                notes.Add(note);
                NoteUIManager.instance.DisplayInventory();
            }
        }

        public void RemoveNote(Note note)
        {
            if (notes.Contains(note))
            {
                notes.Remove(note);
                NoteUIManager.instance.DisplayInventory();
            }
        }

        public bool HasNote(Note note)
        {
            return notes.Contains(note);
        }
    }
}
