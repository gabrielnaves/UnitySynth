namespace Synth
{
    [System.Serializable]
    public class Envelope
    {
        public double attackTime = .01;
        public double decayTime = 0.01;
        public double releaseTime = 0.02;
        public double startAmplitude = 1;
        public double sustainAmplitude = 0.8;

        public double GetAmplitude(Note note)
        {
            double amplitude = 0;
            double time = note.elapsedTime;

            // Attack
            if (time <= attackTime)
                amplitude = time / attackTime * startAmplitude;
            // Decay
            else if (time <= attackTime + decayTime)
                amplitude = (time - attackTime) / decayTime * (sustainAmplitude - startAmplitude) + startAmplitude;
            // Sustain
            else if (note.on)
                amplitude = sustainAmplitude;
            // Release
            else
            {
                if (note.triggerOffTime < 0)
                    note.triggerOffTime = time;
                amplitude = sustainAmplitude - (time - note.triggerOffTime) / releaseTime * sustainAmplitude;
                if (time - note.triggerOffTime > releaseTime)
                    note.Deactivate();
            }

            return amplitude;
        }
    }
}
