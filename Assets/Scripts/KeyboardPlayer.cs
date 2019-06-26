using UnityEngine;

namespace Synth
{
    public class KeyboardPlayer : MonoBehaviour
    {
        public Instrument[] instruments;
        public double frequency;

        private Note note;

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                if (note != null)
                    note.TurnOff();

                note = new Note(frequency);
                foreach (var instrument in instruments)
                    instrument.AddNote(note);
            }
            if (note != null && !Input.GetKey(KeyCode.Z))
            {
                note.TurnOff();
                note = null;
            }
        }
    }
}
