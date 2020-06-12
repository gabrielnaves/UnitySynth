using System.Collections.Generic;
using UnityEngine;

namespace Synth {

    [RequireComponent(typeof(AudioSource))]
    public class Instrument : MonoBehaviour {

        public Envelope envelope;
        public Oscillator[] additiveOscillators;

        [ViewOnly] public List<Note> notes = new List<Note>();

        AudioSource audioSource;
        Oscillator[] allOscillators;

        const double SAMPLING_FREQENCY = 48000.0;
        const double dt = 1 / SAMPLING_FREQENCY;
        double gain;
        //double time;

        public void AddNote(Note note) => notes.Add(note);

        void Awake() {
            audioSource = GetComponent<AudioSource>();
            allOscillators = GetComponentsInChildren<Oscillator>();
        }

        void Update() {
            gain = audioSource.volume * .1;
            notes.RemoveAll((note) => note.dead);
        }

        // Important note: OnAudioFilterRead runs on a different thread, and some unity features aren't available here
        void OnAudioFilterRead(float[] data, int channels) {

            for (int i = 0; i < data.Length; i += channels) {
                UpdateOscillators();
                data[i] = (float)(gain * EvaluateOscillators());
                if (channels == 2)
                    data[i + 1] = data[i];
            }
        }

        void UpdateOscillators() {
            foreach (var oscillator in allOscillators)
                oscillator.UpdateOscillator(dt);
        }

        double EvaluateOscillators() {
            double result = 0;
            foreach (var oscillator in additiveOscillators)
                result += oscillator.value;
            return result;
        }
    }
}
