using UnityEngine;

namespace Synth {

    public class ContinuousNotePlayer : MonoBehaviour {

        public bool playOnStart;
        public Instrument[] instruments;
        public Note note;

        public bool isPlaying { get; private set; }

        Note noteCopy;

        void Start() {
            if (playOnStart)
                Play();
        }

        void OnDestroy() {
            Stop();
        }

        public void Play() {
            isPlaying = true;
            noteCopy = new Note(note);
            foreach (var instrument in instruments)
                instrument.AddNote(noteCopy);
        }

        public void Stop() {
            isPlaying = false;
            noteCopy?.TurnOff();
            noteCopy = null;
        }

        public void Restart() {
            Stop();
            Play();
        }

        public void Toggle() {
            if (isPlaying)
                Stop();
            else
                Play();
        }
    }
}

