namespace Synth
{
    [System.Serializable]
    public class Envelope
    {
        public double attackTime;
        public double decayTime;
        public double releaseTime;
        public double startAmplitude;
        public double sustainAmplitude;

        [ViewOnly] public double triggerOnTime;
        [ViewOnly] public double triggerOffTime;
        [ViewOnly] public bool noteOn;

        public Envelope()
        {
            attackTime = .01;
            decayTime = .01;
            startAmplitude = 1;
            sustainAmplitude = .8;
            releaseTime = 0.02;
            triggerOnTime = 0;
            triggerOffTime = 0;
            noteOn = false;
        }

        public double GetAmplitude(double time)
        {
            double amplitude = 0;
            double lifetime = time - triggerOnTime;

            if (noteOn)
            {
                // Attack
                if (lifetime <= attackTime)
                    amplitude = lifetime / attackTime * startAmplitude;
                // Decay
                else if (lifetime <= attackTime + decayTime)
                    amplitude = (lifetime - attackTime) / decayTime * (sustainAmplitude - startAmplitude) + startAmplitude;
                // Sustain
                else
                    amplitude = sustainAmplitude;
            }
            else
            {
                // Release
                amplitude = sustainAmplitude - (time - triggerOffTime) / releaseTime * sustainAmplitude;
            }

            if (amplitude < .0001)
                amplitude = 0;

            return amplitude;
        }

        public void NoteOn(double time)
        {
            triggerOnTime = time;
            noteOn = true;
        }

        public void NoteOff(double time)
        {
            triggerOffTime = time;
            noteOn = false;
        }
    }
}
