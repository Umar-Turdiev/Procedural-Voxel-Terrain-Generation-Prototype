using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class ChunkRenderer : MonoBehaviour
{
    private MeshFilter _meshFilter;
    private MeshCollider _meshCollider;
    private Mesh _mesh;

    public bool ShowGizmo = false;

    public ChunkData ChunkData { get; private set; }
    public bool IsModifiedByPlayer
    {
        get
        {
            return this.ChunkData.IsModifiedByPlayer;
        }
        set
        {
            this.ChunkData.IsModifiedByPlayer = value;
        }
    }

    private void Awake()
    {
        this._meshFilter = GetComponent<MeshFilter>();
        this._meshCollider = GetComponent<MeshCollider>();
        this._mesh = this._meshFilter.mesh;
    }

    public void InitializeChunk(ChunkData chunkData)
    {
        this.ChunkData = chunkData;
    }

    private void RenderMesh(MeshData meshData)
    {
        this._mesh.Clear();

        this._mesh.subMeshCount = 2;    /* Solid mesh and water/air mesh. */
        this._mesh.vertices = meshData.Vertices.Concat(meshData.WaterMesh.Vertices).ToArray();

        this._mesh.SetTriangles(meshData.Triangles.ToArray(), 0);   /* Triangles in the solid mesh. */

        /* Note: We cannot use the triangle indices directly from the water mesh because the index also starts at 0. 
                 This will overlap with the vertices in the mesh it self. To solve this issue, we use the "select" 
                 from the Linq library to quarry a list of (index + vertex count of the main mesh) to offset the indices. */
        this._mesh.SetTriangles(meshData.WaterMesh.Triangles.Select(val => val + meshData.Vertices.Count()).ToArray(), 1);

        this._mesh.uv = meshData.UVs.Concat(meshData.WaterMesh.UVs).ToArray();
        this._mesh.RecalculateNormals();

        this._meshCollider.sharedMesh = null;
        Mesh collisionMesh = new Mesh();
        collisionMesh.vertices = meshData.ColliderVertices.ToArray();
        collisionMesh.triangles = meshData.ColliderTriangles.ToArray();
        collisionMesh.RecalculateNormals();

        this._meshCollider.sharedMesh = collisionMesh;
    }

    public void UpdateChunk()
    {
        this.RenderMesh(Chunk.GetChunkMeshData(this.ChunkData));
    }

    public void UpdateChunk(MeshData meshData)
    {
        this.RenderMesh(meshData);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (this.ShowGizmo)
        {
            if (Application.isPlaying && ChunkData != null)
            {
                if (Selection.activeObject == this.gameObject)
                {
                    Gizmos.color = new Color(0, 1, 0, 0.5f);
                }
                else
                {
                    Gizmos.color = new Color(1, 0, 1, 0.5f);
                }

                Gizmos.DrawCube(this.transform.position + new Vector3(this.ChunkData.ChunkWidth / 2f,
                this.ChunkData.ChunkHeight / 2f, this.ChunkData.ChunkWidth / 2f), new Vector3(this.ChunkData.ChunkWidth,
                this.ChunkData.ChunkHeight, this.ChunkData.ChunkWidth));
            }
        }
    }
#endif
}
