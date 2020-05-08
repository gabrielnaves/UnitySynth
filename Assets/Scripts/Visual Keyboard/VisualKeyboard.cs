using System;
using UnityEngine;

namespace Synth
{
    public class VisualKeyboard : MonoBehaviour
    {
        public Instrument[] instruments;
        public double baseFrequency;
        public LayerMask keyLayer;

        private VisualKeyboardKey[] keys;
        private Note[] notes;
        private double d12thRootOf2 = Math.Pow(2.0, 1.0 / 12.0);

        private Camera cam;

        private void Start()
        {
            cam = Camera.main;
            keys = GetComponentsInChildren<VisualKeyboardKey>();
            for (int i = 0; i < keys.Length; ++i)
                keys[i].index = i;
            notes = new Note[keys.Length];
        }

        private void Update()
        {
            ResetKeys();
            CheckForPressedKeys();
            UpdateNotes();
        }

        private void ResetKeys()
        {
            foreach (var key in keys)
                key.SetNotPressed();
        }

        private void CheckForPressedKeys()
        {
            if (Input.GetMouseButton(0))
                RaycastAt(cam.ScreenToWorldPoint(Input.mousePosition));
            foreach (var touch in Input.touches) {
                if (touch.phase != TouchPhase.Canceled && touch.phase != TouchPhase.Ended)
                    RaycastAt(cam.ScreenToWorldPoint(touch.position));
            }
        }

        private void RaycastAt(Vector2 position)
        {
            var hit = Physics2D.Raycast(position, Vector2.zero, 0f, keyLayer, -5, 5);
            if (hit)
            {
                var key = hit.collider.GetComponent<VisualKeyboardKey>();
                if (key != null)
                    key.SetPressed();
            }
        }

        private void UpdateNotes()
        {
            for (int i = 0; i < keys.Length; ++i)
            {
                if (notes[i] == null && keys[i].isPressed)
                {
                    notes[i] = new Note(baseFrequency * Math.Pow(d12thRootOf2, i));
                    AddNoteToInstruments(notes[i]);
                }
                else if (notes[i] != null && !keys[i].isPressed)
                {
                    notes[i].TurnOff();
                    notes[i] = null;
                }
            }
        }

        private void AddNoteToInstruments(Note note)
        {
            foreach (var instrument in instruments)
                instrument.AddNote(note);
        }
    }
}
