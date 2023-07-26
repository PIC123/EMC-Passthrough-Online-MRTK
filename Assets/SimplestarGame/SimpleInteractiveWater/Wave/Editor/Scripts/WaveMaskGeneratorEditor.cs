using UnityEditor;
using UnityEngine;

namespace SimplestarGame.Wave
{
    [CustomEditor(typeof(WaveMaskGen))]
    public class WaveMaskGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var waveMaskGenerator = this.target as WaveMaskGen;
            if (GUILayout.Button("Generate"))
            {
                waveMaskGenerator.Generate();
            }
        }
    }
}