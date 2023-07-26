using UnityEngine;

namespace SimplestarGame.Mesh
{
    [RequireComponent(typeof(MeshFilter))]
    public class QuadPlaneMesh : MonoBehaviour
    {
        internal void SubDivide(int width)
        {
            var subDivIndices = new int[6 * width * width];
            var subDivVerts = new Vector3[4 * width * width];
            var subDivUvs = new Vector2[4 * width * width];
            float originalEdgeLength = 2.0f;
            var offsetDelta = 1f / width;
            for (int xIndex = 0; xIndex < width; xIndex++)
            {
                var offsetX = offsetDelta * xIndex;
                for (int yIndex = 0; yIndex < width; yIndex++)
                {
                    var offsetY = offsetDelta * yIndex;
                    var offsetIndex = width * xIndex + yIndex;

                    var leftBottom = new Vector3(offsetX - 0.5f, 0, offsetY - 0.5f) * originalEdgeLength;
                    var rightBottom = leftBottom + new Vector3(offsetDelta, 0, 0) * originalEdgeLength;
                    var leftUp = leftBottom + new Vector3(0, 0, offsetDelta) * originalEdgeLength;
                    var rightUp = leftBottom + new Vector3(offsetDelta, 0, offsetDelta) * originalEdgeLength;

                    subDivVerts[4 * offsetIndex + 0] = leftBottom;
                    subDivVerts[4 * offsetIndex + 1] = rightBottom;
                    subDivVerts[4 * offsetIndex + 2] = leftUp;
                    subDivVerts[4 * offsetIndex + 3] = rightUp;

                    var uvLeftBottom = new Vector2(offsetX, offsetY);
                    var uvRightBottom = uvLeftBottom + new Vector2(offsetDelta, 0);
                    var uvLeftUp = uvLeftBottom + new Vector2(0, offsetDelta);
                    var uvRightUp = uvLeftBottom + new Vector2(offsetDelta, offsetDelta);

                    subDivUvs[4 * offsetIndex + 0] = uvLeftBottom;
                    subDivUvs[4 * offsetIndex + 1] = uvRightBottom;
                    subDivUvs[4 * offsetIndex + 2] = uvLeftUp;
                    subDivUvs[4 * offsetIndex + 3] = uvRightUp;

                    subDivIndices[6 * offsetIndex + 0] = 4 * offsetIndex + 0;
                    subDivIndices[6 * offsetIndex + 1] = 4 * offsetIndex + 3;
                    subDivIndices[6 * offsetIndex + 2] = 4 * offsetIndex + 1;
                    subDivIndices[6 * offsetIndex + 3] = 4 * offsetIndex + 3;
                    subDivIndices[6 * offsetIndex + 4] = 4 * offsetIndex + 0;
                    subDivIndices[6 * offsetIndex + 5] = 4 * offsetIndex + 2;
                }
            }

            var subDivMesh = new UnityEngine.Mesh();
            subDivMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            subDivMesh.name = SubDivMeshName + this.name;
            subDivMesh.SetVertices(subDivVerts);
            subDivMesh.SetTriangles(subDivIndices, 0);
            subDivMesh.SetUVs(0, subDivUvs);
            subDivMesh.RecalculateBounds();
            subDivMesh.RecalculateNormals();
            subDivMesh.RecalculateTangents();

            if (this.gameObject.TryGetComponent(out MeshFilter meshFilter))
            {
                if (this.created)
                {
                    meshFilter.mesh.Clear();
                }
                this.created = true;
                meshFilter.mesh = subDivMesh;
            }
        }

        bool created = false;
        const string SubDivMeshName = "ScriptGenerated";
    }
}