using UnityEngine;

namespace Synth
{
    public class ContinuousNotePlayer : MonoBehaviour
    {
        public Instrument[] instruments;
        public Note note;

        private void Start()
        {
            foreach (var instrument in instruments)
                instrument.AddNote(note);
        }

        private void OnDestroy()
        {
            note.TurnOff();
        }
    }
}

