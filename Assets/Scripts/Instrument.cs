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
        private Oscillator[] oscillators;
        private double gain;
        private double sampling_frequency = 48000.0;
        private double dt;
        private double time;

        public void AddNote(Note note)
        {
            notes.Add(note);
        }

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            oscillators = GetComponents<Oscillator>();
            dt = 1 / sampling_frequency;
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
                UpdatePhases(notes);

                double d = 0;
                foreach (var note in notes)
                {
                    foreach (var oscillator in oscillators)
                    {
                        d += envelope.GetAmplitude(note) * oscillator.Evaluate(note, time);
                    }
                }

                data[i] = (float)(gain * d);
                if (channels == 2)
                    data[i + 1] = data[i];
            }
        }

        private void UpdatePhases(Note[] notes)
        {
            time += dt;
            foreach (var oscillator in oscillators)
                oscillator.UpdatePhase(dt);
            foreach (var note in notes)
                note.Update(dt);
        }
    }
}
