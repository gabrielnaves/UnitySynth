using UnityEngine;

namespace Synth
{
    // Original by Dano Kablamo (video tutorial at https://www.youtube.com/watch?v=GqHFGMy_51c)
    // Additional functionality by Gabriel Naves
    [RequireComponent(typeof(AudioSource))]
    public class Oscillator : MonoBehaviour
    {
        private AudioSource audioSource;
        private Waveform[] waveforms;
        private double gain;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            waveforms = GetComponents<Waveform>();
        }

        private void Update()
        {
            gain = audioSource.volume * .1;
        }

        private void OnAudioFilterRead(float[] data, int channels)
        {
            foreach (var wave in waveforms)
                wave.UpdateIncrement();
            for (int i = 0; i < data.Length; i += channels)
            {
                double d = 0;
                foreach (var wave in waveforms)
                {
                    wave.UpdatePhase();
                    d += wave.Evaluate();
                }
                data[i] = (float)(gain * d);
                if (channels == 2)
                    data[i + 1] = data[i];
            }
        }
    }
}
