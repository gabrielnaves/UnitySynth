using System.Collections.Generic;
using UnityEngine;

namespace Synth
{
    [RequireComponent(typeof(AudioSource))]
    public class Instrument : MonoBehaviour
    {
        public Envelope envelope;

        private AudioSource audioSource;
        private Oscillator[] waveforms;
        private List<Note> notes = new List<Note>();
        private double gain;

        public void AddNote(Note note)
        {
            notes.Add(note);
        }

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            waveforms = GetComponents<Oscillator>();
        }

        private void Update()
        {
            gain = audioSource.volume * .1;
            notes.RemoveAll((note) => note.dead);
        }

        // Important note: OnAudioFilterRead runs on a different thread, and some unity features aren't available here
        private void OnAudioFilterRead(float[] data, int channels)
        {
            Note[] notes = this.notes.ToArray();

            for (int i = 0; i < data.Length; i += channels)
            {
                double d = 0;
                foreach (var wave in waveforms)
                {
                    wave.UpdateLFOPhase();
                    foreach (var note in notes)
                    {
                        note.Update();
                        d += envelope.GetAmplitude(note) * wave.Evaluate(note);
                    }
                }

                data[i] = (float)(gain * d);
                if (channels == 2)
                    data[i + 1] = data[i];
            }
        }
    }
}
