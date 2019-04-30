using System.Collections.Generic;
using UnityEngine;

// Original by Dano Kablamo (video tutorial at https://www.youtube.com/watch?v=GqHFGMy_51c)
// Additional functionality by Gabriel Naves
[RequireComponent(typeof(AudioSource))]
public class Oscillator : MonoBehaviour
{
    public enum WaveType { sine, square, triangle, sawtooth, animationCurve }

    public WaveType waveType;
    public double frequency = 440.0;
    public AnimationCurve curve;
    [Range(0, 1)] public float volume = 1;

    private double increment;
    private double phase;
    private double gain;
    private double sampling_frequency = 48000.0;

    private float pi_twice = Mathf.PI * 2;

    private Dictionary<WaveType, System.Func<float>> waveFunction;

    private void Awake()
    {
        CreateWaveFunctionDictionary();
    }

    private void CreateWaveFunctionDictionary()
    {
        waveFunction = new Dictionary<WaveType, System.Func<float>>();
        waveFunction.Add(WaveType.sine, SineWave);
        waveFunction.Add(WaveType.square, SquareWave);
        waveFunction.Add(WaveType.triangle, TriangleWave);
        waveFunction.Add(WaveType.sawtooth, SawtoothWave);
        waveFunction.Add(WaveType.animationCurve, AnimationCurveWave);
    }

    private void OnAudioFilterRead(float[] data, int channels)
    {
        increment = frequency * pi_twice / sampling_frequency;
        gain = volume * .1;
        for (int i = 0; i < data.Length; i += channels)
        {
            UpdatePhase();
            data[i] = waveFunction[waveType]();
            if (channels == 2)
                data[i + 1] = data[i];
        }
    }

    private void UpdatePhase()
    {
        phase += increment;
        if (phase > pi_twice)
            phase -= pi_twice;
    }

    private float SineWave()
    {
        return (float)(gain * Mathf.Sin((float)phase));
    }

    private float SquareWave()
    {
        return (float)gain * (Mathf.Sin((float)phase) >= 0 ? .5f : -.5f);
    }

    private float TriangleWave()
    {
        return (float)(gain * (double)Mathf.PingPong((float)phase, 1));
    }

    private float SawtoothWave()
    {
        return (float)(gain * (phase / pi_twice - 1));
    }

    private float AnimationCurveWave()
    {
        return (float)(gain * curve.Evaluate((float)(phase / pi_twice)));
    }
}
