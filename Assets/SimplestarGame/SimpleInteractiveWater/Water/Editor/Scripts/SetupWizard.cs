using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;
using System;
using UnityEditor.PackageManager.Requests;
using UnityEditor.PackageManager;

namespace SimplestarGame.Water
{
    public class SetupWizard : ScriptableWizard
    {
        [MenuItem("Window/Simple Interactive Water/Setup Wizard")]
        public static void UpdateSettings()
        {
            DisplayWizard<SetupWizard>("Setup Wizard", "Close");
        }

        void OnWizardCreate()
        {
            // for close
        }

        protected override bool DrawWizardGUI()
        {
            var result = base.DrawWizardGUI();
            this.TitleLayout("Simple Interactive Water");
            var pipelineType = this.GetPipelineType();
            switch (pipelineType)
            {
                case PipelineType.UniversalRP:
                    {
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            EditorGUILayout.PrefixLabel(pipelineType.ToString(), new GUIStyle { fixedWidth = 20 });
                            this.GreenLabel("Supported", 90);
                        }
                        var urp = GraphicsSettings.renderPipelineAsset;
                        if (null != urp)
                        {
                            var serializedObject = new SerializedObject(urp);
                            using (new EditorGUILayout.HorizontalScope())
                            {
                                var key = "m_RequireDepthTexture";
                                serializedObject.FindProperty(key).boolValue = EditorGUILayout.ToggleLeft("Depth Texture",
                                    serializedObject.FindProperty(key).boolValue);
                                if (serializedObject.FindProperty(key).boolValue)
                                {
                                    this.GreenLabel("OK", 20);
                                }
                                else
                                {
                                    this.RedLabel("NG", 20);
                                    if (this.FixButton("Fix Now"))
                                    {
                                        serializedObject.FindProperty(key).boolValue = true;
                                    };
                                }
                            }
                            using (new EditorGUILayout.HorizontalScope())
                            {
                                var key = "m_RequireOpaqueTexture";
                                serializedObject.FindProperty(key).boolValue = EditorGUILayout.ToggleLeft("Opaque Texture",
                                    serializedObject.FindProperty(key).boolValue);
                                if (serializedObject.FindProperty(key).boolValue)
                                {
                                    this.GreenLabel("OK", 20);
                                }
                                else
                                {
                                    this.RedLabel("NG", 20);
                                    if (FixButton("Fix Now"))
                                    {
                                        serializedObject.FindProperty(key).boolValue = true;
                                    };
                                }
                            }
                            using (new EditorGUILayout.HorizontalScope())
                            {
                                var key = "m_OpaqueDownsampling";
                                serializedObject.FindProperty(key).enumValueIndex = Convert.ToInt32(EditorGUILayout.EnumPopup("Opaque Downsampling",
                                    (Downsampling)serializedObject.FindProperty(key).enumValueIndex));
                                if (0 == serializedObject.FindProperty(key).enumValueIndex)
                                {
                                    this.GreenLabel("OK", 20);
                                }
                                else
                                {
                                    this.RedLabel("NG", 20);
                                    if (this.FixButton("Fix Now"))
                                    {
                                        serializedObject.FindProperty(key).enumValueIndex = 0;
                                    };
                                }
                            }
                            if (serializedObject.hasModifiedProperties)
                            {
                                serializedObject.ApplyModifiedProperties();
                            }
                        }
                    }
                    break;
                case PipelineType.HDRP:
                    {
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            EditorGUILayout.PrefixLabel(pipelineType.ToString(), new GUIStyle { fixedWidth = 20 });
                            this.GreenLabel("Supported", 90);
                        }
                    }
                    break;
                default:
                    {
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            EditorGUILayout.PrefixLabel(pipelineType.ToString(), new GUIStyle { fixedWidth = 20 });
                            this.RedLabel("Unsupported", 90);
                            if (this.FixButton("Fix"))
                            {
                                this.SetURP();
                            };
                        }
                    }
                    break;
            }

            return result;
        }

        void SetURP()
        {
            this.request = Client.Add("com.unity.render-pipelines.universal");
            EditorApplication.update += Progress;
        }

        void Progress()
        {
            if (request.IsCompleted)
            {
                if (request.Status == StatusCode.Success)
                {
                    var path = AssetDatabase.GUIDToAssetPath("0d23e74914ee1fe42b28bd1625ae66ce"); // Universal Render Pipeline Asset
                    var urp = AssetDatabase.LoadAssetAtPath<RenderPipelineAsset>(path);
                    GraphicsSettings.renderPipelineAsset = urp;
                }
                else if (request.Status >= StatusCode.Failure)
                {
                    Debug.LogError(request.Error.message);
                }
                EditorApplication.update -= Progress;
            }
        }

        void TitleLayout(string title)
        {
            var setupWizardBackgroundTexture = this.LoadGUIDTexture("3c94ab0c3c885ce488cdc3b22a61efe5"); // Assets/SimplestarGame/SimpleInteractiveWater/Water/Editor/Textures/SetupWizardBackground.png
            using (new EditorGUILayout.VerticalScope(new GUIStyle
            {
                normal = new GUIStyleState
                {
                    background = setupWizardBackgroundTexture,
                }
            }, GUILayout.Height(80f)))
            {
                EditorGUILayout.LabelField("", GUILayout.Height(40f));
                EditorGUILayout.LabelField(title, new GUIStyle
                {
                    padding = new RectOffset(0, 0, 0, 0),
                    alignment = TextAnchor.MiddleCenter,
                    fontSize = 18,
                    normal = new GUIStyleState
                    {
                        textColor = Color.white
                    }
                }, GUILayout.Height(40f));
            }
        }

        Texture2D LoadGUIDTexture(string guid)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            return sprite.texture;
        }

        void GreenLabel(string text, float width)
        {
            var color = GUI.color;
            GUI.color = Color.green;
            EditorGUILayout.LabelField(text, new GUIStyle { normal = new GUIStyleState { textColor = Color.green } }, GUILayout.Width(width));
            GUI.color = color;
        }

        void RedLabel(string text, float width)
        {
            var color = GUI.color;
            GUI.color = new Color(1f, 0.6f, 0.6f, 1f);
            EditorGUILayout.LabelField(text, new GUIStyle { normal = new GUIStyleState { textColor = new Color(1f, 0.6f, 0.6f, 1f) } }, GUILayout.Width(width));
            GUI.color = color;
        }

        bool FixButton(string text)
        {
            var color = GUI.backgroundColor;
            GUI.backgroundColor = new Color(1f, 0.6f, 0.6f, 1f);
            var result = GUILayout.Button(text);
            GUI.backgroundColor = color;
            return result;
        }

        PipelineType GetPipelineType()
        {
            var renderPipeline = GraphicsSettings.renderPipelineAsset;
            if (null == renderPipeline)
            {
                return PipelineType.StandardRP;
            }
            var pipelineType = renderPipeline.GetType();
            if (pipelineType.Name.Contains("HDRenderPipelineAsset"))
            {
                return PipelineType.HDRP;
            }
            if (pipelineType.Name.Contains("UniversalRenderPipelineAsset"))
            {
                return PipelineType.UniversalRP;
            }
            return PipelineType.Unknown;
        }

        enum PipelineType
        {
            StandardRP,
            UniversalRP,
            HDRP,
            Unknown
        }

        public enum Downsampling
        {
            None,
            _2xBilinear,
            _4xBox,
            _4xBilinear
        }

        AddRequest request;
    }
}