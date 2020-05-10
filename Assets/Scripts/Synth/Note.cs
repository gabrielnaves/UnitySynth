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
        [ViewOnly] public double phase;
        [ViewOnly] public bool dead;

        private double pi_twice;

        public Note()
        {
            Setup();
        }

        public Note(double frequency)
        {
            Setup(frequency);
        }

        private void Setup(double frequency=440)
        {
            this.frequency = frequency;
            pi_twice = 2 * Math.PI;
            triggerOffTime = -1;
            on = true;
            dead = false;
        }

        public void Update(double dt)
        {
            elapsedTime += dt;
            phase += frequency * pi_twice * dt;
        }

        public void TurnOff()
        {
            on = false;
        }

        public void Deactivate()
        {
            dead = true;
        }
    }
}
