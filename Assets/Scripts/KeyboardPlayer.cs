using System;
using UnityEngine;

namespace Synth
{
    public class KeyboardPlayer : MonoBehaviour
    {
        public Instrument[] instruments;
        public double baseFrequency;

        private KeyCode[] keys = new KeyCode[] {
            KeyCode.Z,
            KeyCode.S,
            KeyCode.X,
            KeyCode.D,
            KeyCode.C,
            KeyCode.V,
            KeyCode.G,
            KeyCode.B,
            KeyCode.H,
            KeyCode.N,
            KeyCode.J,
            KeyCode.M,
            KeyCode.Comma
        };
        private Note[] notes;
        private double d12thRootOf2 = Math.Pow(2.0, 1.0 / 12.0);

        private void Awake()
        {
            notes = new Note[keys.Length];
        }

        private void Update()
        {
            for (int i = 0; i < keys.Length; ++i)
            {
                if (Input.GetKeyDown(keys[i]))
                {
                    if (notes[i] != null)
                        notes[i].TurnOff();
                    notes[i] = new Note(baseFrequency * Math.Pow(d12thRootOf2, i));
                    foreach (var instrument in instruments)
                        instrument.AddNote(notes[i]);
                }
                else if (notes[i] != null && !Input.GetKey(keys[i]))
                {
                    notes[i].TurnOff();
                    notes[i] = null;
                }
            }
        }
    }
}
