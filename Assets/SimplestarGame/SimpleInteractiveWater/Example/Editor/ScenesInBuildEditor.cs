#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace SimplestarGame.Example
{
    [CustomEditor(typeof(ScenesInBuildSelector))]
    public class ScenesInBuildEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var sceneChanger = this.target as ScenesInBuildSelector;
            if (GUILayout.Button("Reset Scenes in Build"))
            {
                sceneChanger.ResetScenesInBuild();
            }
        }
    }
}
#endif