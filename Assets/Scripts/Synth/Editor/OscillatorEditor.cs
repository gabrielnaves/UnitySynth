using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Synth
{
    [CustomEditor(typeof(Oscillator))]
    public class OscillatorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            //Oscillator oscillator = (Oscillator)target;
            //oscillator.type = (Oscillator.WaveType)EditorGUILayout.EnumPopup("Wave Type", oscillator.type);
        }
    }
}
