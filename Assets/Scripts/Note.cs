using UnityEngine;

namespace Synth
{
    [System.Serializable]
    public class Note
    {
        public double frequency;
        public bool on;

        [ViewOnly] public double elapsedTime;
        [ViewOnly] public double triggerOffTime;
        [ViewOnly] public bool dead;
        [ViewOnly] public double phase;

        private double sampling_frequency = 48000.0;
        private double dt;
        private double pi_twice = 2 * Mathf.PI;
        private double increment;

        public Note(double frequency)
        {
            this.frequency = frequency;
            on = true;
            elapsedTime = 0;
            triggerOffTime = 0;
            dead = false;
            phase = 0;
            dt = 1.0 / sampling_frequency;
            increment = frequency * pi_twice / sampling_frequency;
        }

        public void UpdateTime() {
            elapsedTime += dt;
        }

        public void UpdatePhase()
        {
            phase += increment;
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
