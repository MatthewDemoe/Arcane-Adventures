using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Player
{
    public class WaterOverlay : MonoBehaviour
    {
        private MeshRenderer meshRenderer;

        public bool isOn => meshRenderer.enabled;

        private void Start()
        {
            var meshFilter = GetComponent<MeshFilter>();
            meshRenderer = GetComponent<MeshRenderer>();

            InvertNormals(meshFilter.mesh);
            SwapOrderOfTriangleVertices(meshFilter.mesh);
        }

        public void Set(bool on) => meshRenderer.enabled = on;

        private void InvertNormals(Mesh mesh)
        {
            var normals = mesh.normals;

            for (int i = 0; i < normals.Length; i++)
            {
                normals[i] = -normals[i];
            }

            mesh.normals = normals;
        }

        private void SwapOrderOfTriangleVertices(Mesh mesh)
        {
            for (int i = 0; i < mesh.subMeshCount; i++)
            {
                var triangles = mesh.GetTriangles(i);

                for (int j = 0; j < triangles.Length; j += 3)
                {
                    var temp = triangles[j];
                    triangles[j] = triangles[j + 1];
                    triangles[j + 1] = temp;
                }

                mesh.SetTriangles(triangles, i);
            }
        }
    }
}