using System.Collections.Generic;
using UnityEngine;

namespace Synth
{
    public class Waveform : MonoBehaviour
    {
        public enum WaveType { sine, square, triangle, sawtooth, analogSawtooth, animationCurve, noise }

        public WaveType type;
        public AnimationCurve curve;
        public double lfoFrequency;
        public double lfoAmplitude;

        private double sampling_frequency = 48000.0;
        private double pi_twice = 2 * Mathf.PI;
        private double lfoIncrement;
        private double lfoPhase;
        private System.Random rand = new System.Random();

        private Dictionary<WaveType, System.Func<Note, double>> waveFunction;

        public void UpdateLFOIncrement()
        {
            lfoIncrement = lfoFrequency * pi_twice / sampling_frequency;
        }

        public void UpdateLFOPhase()
        {
            lfoPhase += lfoIncrement;
            if (lfoPhase > pi_twice)
                lfoPhase -= pi_twice;
        }

        public double Evaluate(Note note) {
            note.UpdatePhase();
            return waveFunction[type](note);
        }

        private void Awake()
        {
            CreateWaveFunctionDictionary();
        }

        private void CreateWaveFunctionDictionary()
        {
            waveFunction = new Dictionary<WaveType, System.Func<Note, double>>();
            waveFunction.Add(WaveType.sine, SineWave);
            waveFunction.Add(WaveType.square, SquareWave);
            waveFunction.Add(WaveType.triangle, TriangleWave);
            waveFunction.Add(WaveType.sawtooth, SawtoothWave);
            waveFunction.Add(WaveType.analogSawtooth, AnalogSawtoothWave);
            waveFunction.Add(WaveType.animationCurve, AnimationCurveWave);
            waveFunction.Add(WaveType.noise, Noise);
        }

        private double SineWave(Note note)
        {
            return Mathf.Sin((float)(note.phase + lfoAmplitude * Mathf.Sin((float)lfoPhase)));
        }

        private double SquareWave(Note note)
        {
            return Mathf.Sin((float)(note.phase + lfoAmplitude * Mathf.Sin((float)lfoPhase))) >= 0 ? .5f : -.5f;
        }

        private double TriangleWave(Note note)
        {
            return 2 / Mathf.PI * Mathf.Asin(Mathf.Sin((float)(note.phase + lfoAmplitude * Mathf.Sin((float)lfoPhase))));
        }

        private double SawtoothWave(Note note)
        {
            return note.phase / pi_twice - .5;
        }

        private double AnalogSawtoothWave(Note note)
        {
            double output = 0;
            for (double n = 1; n < 10; ++n)
                output += Mathf.Sin((float)(n * (note.phase + lfoAmplitude * Mathf.Sin((float)lfoPhase)))) / n;
            return output;
        }

        private double AnimationCurveWave(Note note)
        {
            return curve.Evaluate((float)(note.phase / pi_twice));
        }

        private double Noise(Note note)
        {
            return rand.NextDouble() * 2 - 1;
        }
    }
}

