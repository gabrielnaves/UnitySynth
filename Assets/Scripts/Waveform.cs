using System.Collections.Generic;
using UnityEngine;

namespace Synth
{
    public class Waveform : MonoBehaviour
    {
        public enum WaveType { sine, square, triangle, sawtooth, animationCurve, noise }

        public WaveType type;
        public AnimationCurve curve;
        public double frequency = 440;
        public double lfoFrequency;
        public double lfoAmplitude;

        private double sampling_frequency = 48000.0;
        private double pi_twice = 2 * Mathf.PI;
        private double increment;
        private double phase;
        private double lfoIncrement;
        private double lfoPhase;
        private System.Random rand = new System.Random();

        private Dictionary<WaveType, System.Func<double>> waveFunction;

        public void UpdateIncrement()
        {
            increment = frequency * pi_twice / sampling_frequency;
            lfoIncrement = lfoFrequency * pi_twice / sampling_frequency;
        }

        public void UpdatePhase()
        {
            phase += increment;
            if (phase > pi_twice)
                phase -= pi_twice;
            lfoPhase += lfoIncrement;
            if (lfoPhase > pi_twice)
                lfoPhase -= pi_twice;
        }

        public double Evaluate() {
            return waveFunction[type]();
        }

        private void Awake()
        {
            CreateWaveFunctionDictionary();
        }

        private void CreateWaveFunctionDictionary()
        {
            waveFunction = new Dictionary<WaveType, System.Func<double>>();
            waveFunction.Add(WaveType.sine, SineWave);
            waveFunction.Add(WaveType.square, SquareWave);
            waveFunction.Add(WaveType.triangle, TriangleWave);
            waveFunction.Add(WaveType.sawtooth, SawtoothWave);
            waveFunction.Add(WaveType.animationCurve, AnimationCurveWave);
            waveFunction.Add(WaveType.noise, Noise);
        }

        private double SineWave()
        {
            return Mathf.Sin((float)(phase + lfoAmplitude * Mathf.Sin((float)lfoPhase)));
        }

        private double SquareWave()
        {
            return Mathf.Sin((float)(phase + lfoAmplitude * Mathf.Sin((float)lfoPhase))) >= 0 ? .5f : -.5f;
        }

        private double TriangleWave()
        {
            return 2 / Mathf.PI * Mathf.Asin(Mathf.Sin((float)(phase + lfoAmplitude * Mathf.Sin((float)lfoPhase))));
        }

        private double SawtoothWave()
        {
            return phase / pi_twice - .5;
        }

        private double AnimationCurveWave()
        {
            return curve.Evaluate((float)(phase / pi_twice));
        }

        private double Noise()
        {
            return rand.NextDouble() * 2 - 1;
        }
    }
}

