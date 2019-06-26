using UnityEngine;

namespace Synth
{
    [System.Serializable]
    public class Note
    {
        public double frequency = 440;
        public bool on = true;

        [ViewOnly] public double elapsedTime;
        [ViewOnly] public double triggerOffTime;
        [ViewOnly] public bool dead;
        [ViewOnly] public double phase;

        private double sampling_frequency = 48000.0;
        private double pi_twice = 2 * Mathf.PI;

        private double dt;

        public Note()
        {
            Setup();
        }

        public Note(double frequency)
        {
            this.frequency = frequency;
            Setup();
        }

        private void Setup() {
            dt = 1.0 / sampling_frequency;
        }

        public void Update() {
            elapsedTime += dt;
            phase += frequency * pi_twice / sampling_frequency;
            if (phase > pi_twice)
                phase -= pi_twice;
        }

        public void TurnOff()
        {
            triggerOffTime = elapsedTime;
            on = false;
        }

        public void Deactivate()
        {
            dead = true;
        }
    }
}
