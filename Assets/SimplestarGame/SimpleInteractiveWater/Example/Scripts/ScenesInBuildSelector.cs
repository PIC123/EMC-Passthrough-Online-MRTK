#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SimplestarGame.Example
{
    public class ScenesInBuildSelector : MonoBehaviour
    {
        [SerializeField] Text textSceneTitle;
        [SerializeField] Button buttonNext;
        [SerializeField] Button buttonBack;
        [SerializeField] string[] sceneNames;
#if UNITY_EDITOR
        [SerializeField] SceneAsset[] sceneAssets;
#endif

        public void ResetScenesInBuild()
        {
#if UNITY_EDITOR
            EditorBuildSettingsScene[] editorBuildSettingsScenes = new EditorBuildSettingsScene[this.sceneAssets.Length + 1];
            this.sceneNames = new string[this.sceneAssets.Length];
            editorBuildSettingsScenes[0] = new EditorBuildSettingsScene(SceneManager.GetActiveScene().path, true);
            for (int sceneIndex = 0; sceneIndex < this.sceneAssets.Length; sceneIndex++)
            {
                var sceneAsset = this.sceneAssets[sceneIndex];
                sceneNames[sceneIndex] = sceneAsset.name;
                string scenePath = AssetDatabase.GetAssetPath(sceneAsset);
                editorBuildSettingsScenes[sceneIndex + 1] = new EditorBuildSettingsScene(scenePath, true);
            }
            EditorBuildSettings.scenes = editorBuildSettingsScenes;
#endif
        }

        void Awake()
        {
            this.rootSceneName = SceneManager.GetActiveScene().name;
            SceneManager.activeSceneChanged += this.OnSceneChanged;
            DontDestroyOnLoad(this.gameObject);
        }

        void Start()
        {
            this.buttonNext.onClick.AddListener(this.OnClickButtonNext);
            this.buttonBack.onClick.AddListener(this.OnClickButtoBack);
            this.LoadCurrentScene();
        }

        void OnSceneChanged(Scene _, Scene loaded)
        {
            if (null != this.textSceneTitle)
            {
                if (this.rootSceneName != loaded.name)
                {
                    this.textSceneTitle.text = loaded.name;
                }
            }
        }

        void OnClickButtonNext()
        {
            this.LoadNextScene(this.currentSceneIndex + 1);
        }
        void OnClickButtoBack()
        {
            this.LoadNextScene(this.currentSceneIndex - 1);
        }

        void LoadNextScene(int nextSceneIndex)
        {
            if (this.sceneNames.Length <= nextSceneIndex)
            {
                nextSceneIndex = 0;
            }
            else if (0 > nextSceneIndex)
            {
                nextSceneIndex = this.sceneNames.Length - 1;
            }
            if (this.currentSceneIndex != nextSceneIndex)
            {
                this.currentSceneIndex = nextSceneIndex;
                this.LoadCurrentScene();
            }
        }

        void LoadCurrentScene()
        {
            if (0 <= this.currentSceneIndex && this.sceneNames.Length > this.currentSceneIndex)
            {
                SceneManager.LoadScene(this.sceneNames[this.currentSceneIndex]);
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                this.LoadNextScene(this.currentSceneIndex + 1);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                this.LoadNextScene(this.currentSceneIndex - 1);
            }
        }

        int currentSceneIndex = 0;
        string rootSceneName = "";
    }
}