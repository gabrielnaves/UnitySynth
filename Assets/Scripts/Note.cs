using System;

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

        private double pi_twice;

        public Note()
        {
            Setup();
        }

        public Note(double frequency)
        {
            Setup();
            this.frequency = frequency;
        }

        private void Setup()
        {
            this.frequency = 440;
            on = true;
            pi_twice = 2 * Math.PI;
        }

        public void Update(double dt)
        {
            elapsedTime += dt;
            phase += frequency * pi_twice * dt;
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
