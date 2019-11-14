using System;
using System.Collections.Generic;
using UnityEngine;

namespace Synth
{
    public class Oscillator : MonoBehaviour
    {
        public enum WaveType { sine, square, triangle, analogSawtooth, digitalSawtooth, animationCurve, noise }

        public WaveType type;
        public AnimationCurve curve;
        [Range(0, 1)] public double volume = 1;
        public double linearFrequencyOffset;
        public double frequencyMultiplier = 1;
        public double lfoAmplitude;
        public double lfoMultiplier;
        public int analogSawDivisions = 5;

        private double pi_twice = 2 * Math.PI;
        private double offsetPhase;
        private System.Random rand = new System.Random();

        private Dictionary<WaveType, System.Func<Note, double, double>> waveFunction;

        public void UpdatePhase(double dt)
        {
            offsetPhase = (offsetPhase + linearFrequencyOffset * pi_twice * dt) % pi_twice;
        }

        public double Evaluate(Note note, double time)
        {
            return volume * waveFunction[type](note, time);
        }

        private void Awake()
        {
            CreateWaveFunctionDictionary();
        }

        private void CreateWaveFunctionDictionary()
        {
            waveFunction = new Dictionary<WaveType, System.Func<Note, double, double>>();
            waveFunction.Add(WaveType.sine, SineWave);
            waveFunction.Add(WaveType.square, SquareWave);
            waveFunction.Add(WaveType.triangle, TriangleWave);
            waveFunction.Add(WaveType.analogSawtooth, AnalogSawtoothWave);
            waveFunction.Add(WaveType.digitalSawtooth, DigitalSawtoothWave);
            waveFunction.Add(WaveType.animationCurve, AnimationCurveWave);
            waveFunction.Add(WaveType.noise, Noise);
        }

        private double SineWave(Note note, double time)
        {
            return Math.Sin((note.phase + offsetPhase) * frequencyMultiplier + lfoAmplitude * Math.Sin(note.frequency * lfoMultiplier * 2 * Math.PI * time));
        }

        private double SquareWave(Note note, double time)
        {
            return SineWave(note, time) > 0 ? .5 : 0;
        }

        private double TriangleWave(Note note, double time)
        {
            return 2 / Math.PI * Math.Asin(SineWave(note, time));
        }

        private double AnalogSawtoothWave(Note note, double time)
        {
            double output = 0;
            for (double n = 1; n < analogSawDivisions; ++n)
                output += Math.Sin(n * ((note.phase + offsetPhase) * frequencyMultiplier + lfoAmplitude * Math.Sin(note.frequency * lfoMultiplier * 2 * Math.PI * time))) / n;
            return output * .5;
        }

        private double DigitalSawtoothWave(Note note, double time)
        {
            return ((note.phase + offsetPhase) * frequencyMultiplier) % pi_twice / pi_twice;
        }

        private double AnimationCurveWave(Note note, double time)
        {
            return curve.Evaluate((float)DigitalSawtoothWave(note, time));
        }

        private double Noise(Note note, double time)
        {
            return rand.NextDouble() * 2 - 1;
        }
    }
}

