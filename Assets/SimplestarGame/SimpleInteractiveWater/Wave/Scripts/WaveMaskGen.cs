#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace SimplestarGame.Wave
{
    public class WaveMaskGen : MonoBehaviour
    {
        [SerializeField] [Range(6, 11)] int resolution = 7;
        [SerializeField] string outputPath = "SimplestarGame/SimpleInteractiveWater/Example/Textures";

        public void Generate()
        {
            if (this.gameObject.TryGetComponent(out MeshFilter meshFilter))
            {
                int width = Mathf.RoundToInt(Mathf.Pow(2, this.resolution));
                int height = width;

                var center = this.transform.position;
                var boundsMax = new Vector3(
                    meshFilter.sharedMesh.bounds.max.x * this.transform.localScale.x,
                    meshFilter.sharedMesh.bounds.max.y * this.transform.localScale.y,
                    meshFilter.sharedMesh.bounds.max.z * this.transform.localScale.z);
                var boundsMin = new Vector3(
                    meshFilter.sharedMesh.bounds.min.x * this.transform.localScale.x,
                    meshFilter.sharedMesh.bounds.min.y * this.transform.localScale.y,
                    meshFilter.sharedMesh.bounds.min.z * this.transform.localScale.z);
                var deltaX = (boundsMax.x - boundsMin.x) / width;
                var deltaZ = (boundsMax.z - boundsMin.z) / height;
                var waterLayer = LayerMask.NameToLayer("Water");
                Texture2D tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
                Color[] colors = new Color[width * height];
                for (int z = 0; z < height; z++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        float X = center.x - boundsMax.x + deltaX * x;
                        float Y = center.y;
                        float Z = center.z - boundsMax.z + deltaZ * z;

                        var origin = new Vector3(X, Y, Z);
                        bool rayX = Physics.Raycast(origin - Vector3.right * deltaX, Vector3.right, deltaX, ~(1 << waterLayer));
                        bool rayX_ = Physics.Raycast(origin + Vector3.right * deltaX, -Vector3.right, deltaX, ~(1 << waterLayer));
                        bool rayY = Physics.Raycast(origin - Vector3.forward * deltaZ, Vector3.forward, deltaZ, ~(1 << waterLayer));
                        bool rayY_ = Physics.Raycast(origin + Vector3.forward * deltaZ, -Vector3.forward, deltaZ, ~(1 << waterLayer));
                        if (rayX || rayX_ || rayY || rayY_)
                        {
                            int index = height * z + x;
                            colors[index] = new Color(0, 1, 0);
                        }
                    }
                }
                tex.SetPixels(0, 0, width, height, colors, 0);
                tex.Apply();
                byte[] bytes = tex.EncodeToPNG();
                Object.DestroyImmediate(tex);
                var assetPath = $"/{this.outputPath}/Mask_{this.gameObject.name}_{this.transform.position.ToString("0")}.png";
                File.WriteAllBytes(Application.dataPath + assetPath, bytes);
                AssetDatabase.Refresh();
                this.EnableReadWriteTexture("Assets" + assetPath);
            }
        }

        void EnableReadWriteTexture(string path)
        {
            var importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (null != importer)
            {
                importer.isReadable = true;
                importer.SaveAndReimport();
                AssetDatabase.Refresh();
            }
        }
    }
}
#endif