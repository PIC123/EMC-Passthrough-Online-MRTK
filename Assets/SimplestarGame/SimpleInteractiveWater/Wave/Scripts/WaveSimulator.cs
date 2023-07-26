using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimplestarGame.Wave
{
    /// <summary>
    /// Wave Simulator
    /// </summary>
    /// <remarks>
    /// original idea is from this qiita topic https://qiita.com/aa_debdeb/items/1d69d49333630b06f6ce
    /// </remarks>
    public class WaveSimulator : MonoBehaviour
    {
        [SerializeField] ComputeShader computeShaderResource;
        [SerializeField][Range(0.01f, 0.07f)] internal float waveSpeed = 0.056f;
        [SerializeField][Range(0.1f, 10.0f)] internal float wavePower = 3f;
        [SerializeField][Range(0.0001f, 0.9f)] internal float dampingForce = 0.01f;
        [SerializeField] Texture2D maskTexture;
        [SerializeField] bool disableAutoResolution = false;

        /// <summary>
        /// Wave Interactive Function
        /// </summary>
        /// <param name="point">AddWavePoint</param>
        /// <param name="radius">radius</param>
        /// <param name="velocity">PointVelocity</param>
        internal void AddWavePoint(Vector3 point, float radius, Vector3 velocity)
        {
            var localPoint = this.transform.worldToLocalMatrix.MultiplyPoint(point);
            localPoint = new Vector3(localPoint.x / this.boundsMax.x, 0, localPoint.z / this.boundsMax.z);
            var waveHeight = this.transform.worldToLocalMatrix.MultiplyVector(velocity);
            float height = waveHeight.y * this.wavePower * Mathf.Min(this.lastScale, 16);
            Vector4 addWavePoint = new Vector4(localPoint.x, localPoint.z, Mathf.Max(radius * 50f, 2f) / this.width, 10f * height / this.width);
            this.addWavePoints.Add(addWavePoint);
            this.lastAddWaveInfo = addWavePoint;
        }

        internal void AddWaveLine(Vector3 point1, Vector3 point2, float radius, Vector3 velocity)
        {
            var localPoint1 = this.transform.worldToLocalMatrix.MultiplyPoint(point1);
            localPoint1 = new Vector3(localPoint1.x / this.boundsMax.x, 0, localPoint1.z / this.boundsMax.z);
            var localPoint2 = this.transform.worldToLocalMatrix.MultiplyPoint(point2);
            localPoint2 = new Vector3(localPoint2.x / this.boundsMax.x, 0, localPoint2.z / this.boundsMax.z);

            var waveHeight = this.transform.worldToLocalMatrix.MultiplyVector(velocity);
            float height = waveHeight.y * this.wavePower * Mathf.Min(this.lastScale, 16);
            Vector4 addWavePoint = new Vector4(localPoint1.x, localPoint1.z, Mathf.Max(radius * 50f, 2f) / this.width, 10f * height / this.width);
            this.addWaveLines.Add(addWavePoint);
            this.addWaveLines.Add(new Vector4(localPoint2.x, localPoint2.z, 0, 0));
            this.lastAddWaveInfo = addWavePoint;
        }

        internal float GetWaveHeight(Vector3 point)
        {
            if (null != this.waveHeightResultBuffer)
            {
                if (null == this.waveHeightResultBufferForCPU || this.waveHeightResultBufferForCPU.Length != this.width * this.width)
                {
                    this.waveHeightResultBufferForCPU = new float[this.width * this.width];
                }
                if (this.updateCounter != this.lastGetWaveHeightUpdateCounter)
                {
                    this.waveHeightResultBuffer.GetData(this.waveHeightResultBufferForCPU);
                    this.lastGetWaveHeightUpdateCounter = this.updateCounter;
                }
                var localPoint = this.transform.worldToLocalMatrix.MultiplyPoint(point);
                localPoint = new Vector3(localPoint.x / this.boundsMax.x, 0, localPoint.z / this.boundsMax.z);
                Vector3 pickPoint = localPoint * 0.5f + new Vector3(0.5f, 0f, 0.5f);
                int pickIndex = Mathf.FloorToInt(pickPoint.z * this.width) * this.width + Mathf.FloorToInt(pickPoint.x * this.width);
                if (0 <= pickIndex && this.waveHeightResultBufferForCPU.Length > pickIndex)
                {
                    return this.waveHeightResultBufferForCPU[pickIndex] * 0.05f * this.transform.localScale.z;
                }
            }
            return 0;
        }

        void Start()
        {
            this.computeShader = Instantiate(this.computeShaderResource);
            this.computeShader.name = this.name;

            // Get ComputeShader Kernel Index;
            this.kernelAddWave = this.computeShader.FindKernel("AddWave");
            this.kernelUpdate = this.computeShader.FindKernel("Update");
            this.kernelReplace = this.computeShader.FindKernel("Replace");

            var scale = Mathf.Max(this.transform.localScale.x, this.transform.localScale.z);
            this.UpdateTextureResolution(scale);

            // Get Thread Size
            uint threadSizeX, threadSizeY, threadSizeZ;
            this.computeShader.GetKernelThreadGroupSizes(this.kernelAddWave, out threadSizeX, out threadSizeY, out threadSizeZ);
            this.threadSizeAddWave = new Vector3Int((int)threadSizeX, (int)threadSizeY, (int)threadSizeZ);
            this.computeShader.GetKernelThreadGroupSizes(this.kernelUpdate, out threadSizeX, out threadSizeY, out threadSizeZ);
            this.threadSizeUpdate = new Vector3Int((int)threadSizeX, (int)threadSizeY, (int)threadSizeZ);
            this.computeShader.GetKernelThreadGroupSizes(this.kernelReplace, out threadSizeX, out threadSizeY, out threadSizeZ);
            this.threadSizeReplace = new Vector3Int((int)threadSizeX, (int)threadSizeY, (int)threadSizeZ);

            // Set empty Neighbors
            var emptyTexture = new RenderTexture(8, 8, 0, RenderTextureFormat.R8);
            emptyTexture.wrapMode = TextureWrapMode.Clamp;
            emptyTexture.enableRandomWrite = true;
            emptyTexture.Create();
            this.computeShader.SetTexture(this.kernelUpdate, this.CurrWaveTextureXId, emptyTexture);
            this.computeShader.SetTexture(this.kernelUpdate, this.CurrWaveTextureZId, emptyTexture);
            this.computeShader.SetTexture(this.kernelUpdate, this.CurrWaveTexture_XId, emptyTexture);
            this.computeShader.SetTexture(this.kernelUpdate, this.CurrWaveTexture_ZId, emptyTexture);

            // Set Default Wave Coef.
            this.computeShader.SetFloat(this.DeltaSizeId, deltaSize);
            this.computeShader.SetFloat(this.WaveCoefId, waveCoef);
            this.computeShader.SetFloat(this.DampingForceId, this.dampingForce);

            this.lastWaveSpeed = this.waveSpeed;
            this.computeShader.SetFloat(this.WaveSpeedId, this.waveSpeed);
            this.computeShader.SetFloats(this.UpVectorId, this.transform.up.x, this.transform.up.y, this.transform.up.z);

            StartCoroutine(this.CoFindNeighbor());

            if (TryGetComponent(out MeshFilter meshFilter))
            {

                if (null != meshFilter.sharedMesh)
                {
                    this.boundsMax = meshFilter.sharedMesh.bounds.max;
                }
            }
        }

        void UpdateTextureResolution(float scale)
        {
            this.lastScale = scale;
            int nextResolution = 7;
            if (!this.disableAutoResolution)
            {
                if (1 > scale)
                {
                    nextResolution = 6;
                }
                else if (2 > scale)
                {
                    nextResolution = 7;
                }
                else if (4 > scale)
                {
                    nextResolution = 8;
                }
                else if (8 > scale)
                {
                    nextResolution = 9;
                }
                else if (16 > scale)
                {
                    nextResolution = 10;
                }
                else
                {
                    nextResolution = 11;
                }
            }
            if(this.resolution == nextResolution)
            {
                // Skip Update
                return;
            }
            this.resolution = nextResolution;
            this.width = null != this.maskTexture ? this.maskTexture.width : Mathf.RoundToInt(Mathf.Pow(2, this.resolution));

            // Create Float Texture x 3
            if (null != this.lastWaveTexture)
            {
                this.lastWaveTexture.Release();
            }
            this.lastWaveTexture = new RenderTexture(this.width, this.width, 0, RenderTextureFormat.ARGBFloat);
            this.lastWaveTexture.wrapMode = TextureWrapMode.Clamp;
            this.lastWaveTexture.enableRandomWrite = true;
            this.lastWaveTexture.Create();

            if (null != this.currWaveTexture)
            {
                this.currWaveTexture.Release();
            }
            this.currWaveTexture = new RenderTexture(this.width, this.width, 0, RenderTextureFormat.ARGBFloat);
            this.currWaveTexture.wrapMode = TextureWrapMode.Clamp;
            this.currWaveTexture.enableRandomWrite = true;
            this.currWaveTexture.Create();

            if (null != this.nextWaveTexture)
            {
                this.nextWaveTexture.Release();
            }
            this.nextWaveTexture = new RenderTexture(this.width, this.width, 0, RenderTextureFormat.ARGBFloat);
            this.nextWaveTexture.wrapMode = TextureWrapMode.Clamp;
            this.nextWaveTexture.enableRandomWrite = true;
            this.nextWaveTexture.Create();

            // Initialize Texture
            var initTexutre = new Texture2D(this.width, this.width, TextureFormat.RG32, false);
            for (int x = 0; x < this.width; x++)
            {
                for (int y = 0; y < this.width; y++)
                {
                    var maskPixel = null != this.maskTexture ? this.maskTexture.GetPixel(x, y) : Color.black;
                    initTexutre.SetPixel(x, y, new Color(0, maskPixel.g, 0));
                }
            }
            initTexutre.Apply();
            Graphics.Blit(initTexutre, this.lastWaveTexture);
            Graphics.Blit(initTexutre, this.currWaveTexture);
            Graphics.Blit(initTexutre, this.nextWaveTexture);
            Destroy(initTexutre);

            // Set to Update Kernel
            this.computeShader.SetTexture(this.kernelUpdate, this.LastWaveTextureId, this.lastWaveTexture);
            this.computeShader.SetTexture(this.kernelUpdate, this.CurrWaveTextureId, this.currWaveTexture);
            this.computeShader.SetTexture(this.kernelUpdate, this.NextWaveTextureId, this.nextWaveTexture);

            if (null != this.waveHeightResultBuffer)
            {
                this.waveHeightResultBuffer.Release();
            }
            this.waveHeightResultBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Raw , this.width * this.width, sizeof(float));
            this.computeShader.SetBuffer(this.kernelUpdate, this.WaveHeightResultId, this.waveHeightResultBuffer);

            //// Set to AddWave Kernel
            this.computeShader.SetTexture(this.kernelAddWave, this.CurrWaveTextureId, this.currWaveTexture);

            //// Set to Replace Kernel
            this.computeShader.SetTexture(this.kernelReplace, this.LastWaveTextureId, this.lastWaveTexture);
            this.computeShader.SetTexture(this.kernelReplace, this.CurrWaveTextureId, this.currWaveTexture);
            this.computeShader.SetTexture(this.kernelReplace, this.NextWaveTextureId, this.nextWaveTexture);

            // Set Result Texture to Material Param
            if (this.TryGetComponent(out Renderer renderer))
            {
                var material = renderer.material;
                if (null != material)
                {
                    material.SetTexture(this.WaveHeightMapId, this.nextWaveTexture);
                    material.SetFloat(this.ResolutionId, this.width);
                }
            }
            if (this.TryGetComponent(out Mesh.QuadPlaneMesh quadMesh))
            {
                quadMesh.SubDivide(this.width);
            }
        }

        IEnumerator CoFindNeighbor()
        {
            yield return null;
            var others = GameObject.FindObjectsOfType<WaveSimulator>();
            foreach (var other in others)
            {
                var vector = other.transform.position - this.transform.position;
                if (0 == Mathf.RoundToInt(vector.y))
                {
                    Vector2Int grid = new Vector2Int(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.z));
                    int maxDiff = Mathf.Max(Mathf.Abs(grid.x), Mathf.Abs(grid.y));
                    if (maxDiff == Mathf.RoundToInt(this.boundsMax.x) * 2)
                    {
                        var neighbor = other.GetComponent<WaveSimulator>();
                        if (0 < grid.x && 0 == grid.y)
                        {
                            this.computeShader.SetTexture(this.kernelUpdate, this.CurrWaveTextureXId, neighbor.currWaveTexture);
                        }
                        if (0 == grid.x && 0 < grid.y)
                        {
                            this.computeShader.SetTexture(this.kernelUpdate, this.CurrWaveTextureZId, neighbor.currWaveTexture);
                        }
                        if (0 > grid.x && 0 == grid.y)
                        {
                            this.computeShader.SetTexture(this.kernelUpdate, this.CurrWaveTexture_XId, neighbor.currWaveTexture);
                        }
                        if (0 == grid.x && 0 > grid.y)
                        {
                            this.computeShader.SetTexture(this.kernelUpdate, this.CurrWaveTexture_ZId, neighbor.currWaveTexture);
                        }

                    }
                    else if (0 == maxDiff)
                    {
                        var neighbor = other.GetComponent<WaveSimulator>();
                        var neighborUp = this.transform.worldToLocalMatrix.MultiplyVector(neighbor.transform.up);
                        neighborUp = neighborUp.normalized;
                        if (0.8f < neighborUp.x)
                        {
                            this.computeShader.SetTexture(this.kernelUpdate, this.CurrWaveTextureXId, neighbor.currWaveTexture);
                        }
                        if (0.8f < neighborUp.z)
                        {
                            this.computeShader.SetTexture(this.kernelUpdate, this.CurrWaveTextureZId, neighbor.currWaveTexture);
                        }
                        if (-0.8f > neighborUp.x)
                        {
                            this.computeShader.SetTexture(this.kernelUpdate, this.CurrWaveTexture_XId, neighbor.currWaveTexture);
                        }
                        if (-0.8f > neighborUp.z)
                        {
                            this.computeShader.SetTexture(this.kernelUpdate, this.CurrWaveTexture_ZId, neighbor.currWaveTexture);
                        }
                    }
                }
            }

            if (null == WaveUpdater.Instance)
            {
                Debug.LogError("Create WaveUpdater GameObject and attatch WaveUpdater.cs ScriptComponent.");
                yield break;
            }
            WaveUpdater.Instance.onAddWaves += this.AddWaves;
            WaveUpdater.Instance.onUpdateTexture += this.UpdateTextture;
            WaveUpdater.Instance.onReplaceTexture += this.ReplaceTextture;
        }

        void AddWaves()
        {
            int addCount = 0;
            if (0 < this.addWavePoints.Count)
            {
                while (WaveSimulator.MaxAddPoints < this.addWavePoints.Count)
                {
                    this.addWavePoints.RemoveAt(0);
                }
                var addWavePointArray = this.addWavePoints.ToArray();
                this.addWavePoints.Clear();
                this.computeShader.SetInt(this.AddWavePointsCountId, addWavePointArray.Length);
                this.computeShader.SetVectorArray(this.AddWavePointsId, addWavePointArray);
                addCount++;
            }
            if (0 < this.addWaveLines.Count)
            {
                while (WaveSimulator.MaxAddLines < this.addWaveLines.Count)
                {
                    this.addWaveLines.RemoveAt(0);
                }
                var addWaveLineArray = this.addWaveLines.ToArray();
                this.addWaveLines.Clear();
                this.computeShader.SetInt(this.AddWaveLinesCountId, addWaveLineArray.Length);
                this.computeShader.SetVectorArray(this.AddWaveLinesId, addWaveLineArray);
                addCount++;
            }
            if (0 < addCount)
            {
                this.computeShader.Dispatch(this.kernelAddWave, Mathf.CeilToInt(this.currWaveTexture.width / this.threadSizeAddWave.x), Mathf.CeilToInt(this.currWaveTexture.height / this.threadSizeAddWave.y), 1);
            }
        }

        void UpdateTextture()
        {
            if (this.lastWaveSpeed != this.waveSpeed)
            {
                this.lastWaveSpeed = this.waveSpeed;
                this.computeShader.SetFloat(this.WaveSpeedId, this.waveSpeed);
            }
            if (this.lastDampingForce != this.dampingForce)
            {
                this.lastDampingForce = this.dampingForce;
                this.computeShader.SetFloat(this.DampingForceId, this.dampingForce);
            }

            var scale = Mathf.Max(this.transform.localScale.x, this.transform.localScale.z);
            if (this.lastScale != scale)
            {
                this.UpdateTextureResolution(scale);
            }
            // Compute Next Wave Heights
            this.computeShader.Dispatch(this.kernelUpdate, Mathf.CeilToInt(this.currWaveTexture.width / this.threadSizeUpdate.x), Mathf.CeilToInt(this.currWaveTexture.height / this.threadSizeUpdate.y), 1);
            this.updateCounter++;
        }

        void ReplaceTextture()
        {
            this.computeShader.Dispatch(this.kernelReplace, Mathf.CeilToInt(this.currWaveTexture.width / this.threadSizeReplace.x), Mathf.CeilToInt(this.currWaveTexture.height / this.threadSizeReplace.y), 1);
        }

        void OnDestroy()
        {
            this.lastWaveTexture.Release();
            this.currWaveTexture.Release();
            this.nextWaveTexture.Release();
            this.waveHeightResultBuffer.Release();
        }

        /// <summary>
        /// sqrt(width)
        /// </summary>
        int resolution = 0;
        Vector4 lastAddWaveInfo = Vector4.zero;
        const int MaxAddPoints = 128;
        List<Vector4> addWavePoints = new List<Vector4>();
        const int MaxAddLines = 128;
        List<Vector4> addWaveLines = new List<Vector4>();
        float lastWaveSpeed = 0;
        float lastDampingForce = 0.04f;
        float lastScale = 1f;
        const float deltaSize = 0.1f;
        const float waveCoef = 1.0f;
        Vector3 boundsMax = Vector3.one;

        readonly int WaveHeightMapId = Shader.PropertyToID("_WaveHeightMap");
        readonly int ResolutionId = Shader.PropertyToID("_Resolution");

        readonly int LastWaveTextureId = Shader.PropertyToID("_LastWaveTexture");
        readonly int CurrWaveTextureId = Shader.PropertyToID("_CurrWaveTexture");
        readonly int NextWaveTextureId = Shader.PropertyToID("_NextWaveTexture");
        readonly int WaveHeightResultId = Shader.PropertyToID("_WaveHeightResult");

        readonly int CurrWaveTextureXId = Shader.PropertyToID("_NearWaveTextureX");
        readonly int CurrWaveTexture_XId = Shader.PropertyToID("_NearWaveTexture_X");
        readonly int CurrWaveTextureZId = Shader.PropertyToID("_NearWaveTextureZ");
        readonly int CurrWaveTexture_ZId = Shader.PropertyToID("_NearWaveTexture_Z");

        readonly int WaveCoefId = Shader.PropertyToID("_WaveCoef");
        readonly int DeltaSizeId = Shader.PropertyToID("_DeltaSize");
        readonly int WaveSpeedId = Shader.PropertyToID("_WaveSpeed");
        readonly int DampingForceId = Shader.PropertyToID("_DampingForce");
        readonly int UpVectorId = Shader.PropertyToID("_UpVector");
        readonly int AddWavePointsCountId = Shader.PropertyToID("_AddWavePointsCount");
        readonly int AddWavePointsId = Shader.PropertyToID("_AddWavePoints");
        readonly int AddWaveLinesCountId = Shader.PropertyToID("_AddWaveLinesCount");
        readonly int AddWaveLinesId = Shader.PropertyToID("_AddWaveLines");

        /// <summary>
        /// Compute Shader for Wave Simulation
        /// </summary>
        ComputeShader computeShader;
        RenderTexture lastWaveTexture;
        RenderTexture currWaveTexture;
        RenderTexture nextWaveTexture;
        GraphicsBuffer waveHeightResultBuffer;
        float[] waveHeightResultBufferForCPU;
        /// <summary>
        /// ComputeShader Kernels
        /// </summary>
        int kernelAddWave;
        int kernelUpdate;
        int kernelReplace;
        /// <summary>
        /// Quad Mesh Resolution
        /// </summary>
        int width = 0;
        uint updateCounter = 0;
        uint lastGetWaveHeightUpdateCounter = 0;
        /// <summary>
        /// ThreadSize
        /// </summary>
        Vector3Int threadSizeAddWave;
        Vector3Int threadSizeUpdate;
        Vector3Int threadSizeReplace;
    }
}