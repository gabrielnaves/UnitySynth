using System.Collections.Generic;
using UnityEngine;

namespace Synth
{
    [RequireComponent(typeof(AudioSource))]
    public class Instrument : MonoBehaviour
    {
        public Envelope envelope;
        public List<Note> notes = new List<Note>();

        private AudioSource audioSource;
        private Waveform[] waveforms;
        private double gain;

        public void AddNote(Note note)
        {
            notes.Add(note);
        }

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            waveforms = GetComponents<Waveform>();
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

            UpdateWaveLFOIncrements();
            for (int i = 0; i < data.Length; i += channels)
            {
                // Calculate sound value
                double d = 0;
                foreach (var wave in waveforms)
                {
                    wave.UpdateLFOPhase();
                    foreach (var note in notes)
                        d += envelope.GetAmplitude(note) * wave.Evaluate(note);
                }

                // Set sound data values in channels
                data[i] = (float)(gain * d);
                if (channels == 2)
                    data[i + 1] = data[i];
            }
        }

        private void UpdateWaveLFOIncrements()
        {
            foreach (var wave in waveforms)
                wave.UpdateLFOIncrement();
        }
    }
}
