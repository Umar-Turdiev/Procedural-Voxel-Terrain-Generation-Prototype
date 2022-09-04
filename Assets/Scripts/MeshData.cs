using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshData
{
    public List<Vector3> Vertices = new List<Vector3>();    /* Coordinates of the vertices. */
    public List<int> Triangles = new List<int>();
    public List<Vector2> UVs = new List<Vector2>();         /* UV coordinates for the textures. */

    /* The reason we're having separate lists to store the collider coordinates it's because a chunk may contain water mesh 
       that allow player to pass through. */
    public List<Vector3> ColliderVertices = new List<Vector3>();    /* Coordinates of the vertices collider. */
    public List<int> ColliderTriangles = new List<int>();

    public MeshData WaterMesh;

    private bool _isMainMesh = true;

    public MeshData(bool isMainMesh)
    {
        if (isMainMesh)
        {
            this.WaterMesh = new MeshData(false);
        }
    }

    public void AddVertex(Vector3 vertex, bool generateVertexCollider)
    {
        this.Vertices.Add(vertex);

        /* We don't generate collider for water or air. */
        if (generateVertexCollider)
        {
            this.ColliderVertices.Add(vertex);
        }
    }

    public void AddQuadTriangles(bool generateQuadCollider)
    {
        /* Note: Create triangles clockwise so face normal will be facing up. */

        /* Upper triangle. */
        this.Triangles.Add(this.Vertices.Count - 4);
        this.Triangles.Add(this.Vertices.Count - 3);
        this.Triangles.Add(this.Vertices.Count - 2);

        /* Lower triangle. */
        this.Triangles.Add(this.Vertices.Count - 4);
        this.Triangles.Add(this.Vertices.Count - 2);
        this.Triangles.Add(this.Vertices.Count - 1);

        /* We don't generate collider for water or air. */
        if (generateQuadCollider)
        {
            /* Upper triangle. */
            this.ColliderTriangles.Add(this.Vertices.Count - 4);
            this.ColliderTriangles.Add(this.Vertices.Count - 3);
            this.ColliderTriangles.Add(this.Vertices.Count - 2);

            /* Lower triangle. */
            this.ColliderTriangles.Add(this.Vertices.Count - 4);
            this.ColliderTriangles.Add(this.Vertices.Count - 2);
            this.ColliderTriangles.Add(this.Vertices.Count - 1);
        }
    }
}
