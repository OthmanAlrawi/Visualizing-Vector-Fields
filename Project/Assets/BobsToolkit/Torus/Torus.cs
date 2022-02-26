/**
 * Based on a script by Steffen (http://forum.unity3d.com/threads/torus-in-unity.8487/) (in $primitives_966_104.zip, originally named "Primitives.cs")
 *
 * Editted by Michael Zoller on December 6, 2015.
 * It was shortened by about 30 lines (and possibly sped up by a factor of 2) by consolidating math & loops and removing intermediate Collections.
 * 
 * Edit by Bob Jeltes https://github.com/emsylf on November 23, 2020: 
 * TorusCreator.cs is the class that allows the user to create a torus shape like any other basic Unity3D shape, through the GameObject/3D Object menu at the top of the window
 * Torus.cs is now its own class governing everything that has to do with the instance of a torus. 
 * TorusEditor.cs creates a custom inspector that allows you to see the effect of the changing variables in real-time.
 */
using UnityEngine;

[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
public class Torus: MonoBehaviour {
	public float radius = 1f;
	public float thickness = 0.4f;
	public int segments = 32;
	public int segmentDetail = 12;

    private int vertices { get => segments * segmentDetail; }
    private int primitives { get => vertices * 2; }
    private int triangleIndices { get => primitives * 3; }

    MeshFilter mf;
    public MeshFilter Mf
    {
        get
        {
            if (mf == null)
            {
                mf = GetComponent<MeshFilter>();
            }
            return mf;
        }
    }

    private void Awake()
    {
        NewMesh();
    }
    Mesh mesh;
    public Mesh GetMesh()
    {
        mesh = Mf.sharedMesh;
        if (mesh == null)
        {
            mesh = CreateNewMesh();
        }
        return mesh;
    }

    public Mesh CreateNewMesh()
    {
        mesh = new Mesh();
        mesh.name = "Torus";
        Mf.sharedMesh = mesh;
        return mesh;
    }

    public void UpdateTorus()
    {
        Recalculate(out Vector3[] verts, out int[] indices);
        UpdateMesh(verts, indices);
        UpdateMeshCollider();
    }

    public void Recalculate(out Vector3[] vertices, out int[] triangleIndices)
    {
        vertices = new Vector3[this.vertices];
        triangleIndices = new int[this.triangleIndices];

        // Calculate size of a segment and a tube
        float segmentSize = 2 * Mathf.PI / segments;
        float tubeSize = 2 * Mathf.PI / segmentDetail;

        // Create floats for our xyz coordinates
        float x, y, z;

        // Begin loop that fills in both arrays
        for (int i = 0; i < segments; i++)
        {
            // Find next (or first) segment offset
            int n = (i + 1) % segments; // changed segmentList.Count to numSegments

            // Find the current and next segments
            int currentTubeOffset = i * segmentDetail;
            int nextTubeOffset = n * segmentDetail;

            for (int j = 0; j < segmentDetail; j++)
            {
                // Find next (or first) vertex offset
                int m = (j + 1) % segmentDetail; // changed currentTube.Count to numTubes

                // Find the 4 vertices that make up a quad
                int iv1 = currentTubeOffset + j;
                int iv2 = currentTubeOffset + m;
                int iv3 = nextTubeOffset + m;
                int iv4 = nextTubeOffset + j;

                // Calculate X, Y, Z coordinates.
                x = (radius + thickness * Mathf.Cos(j * tubeSize)) * Mathf.Cos(i * segmentSize);
                z = (radius + thickness * Mathf.Cos(j * tubeSize)) * Mathf.Sin(i * segmentSize);
                y = thickness * Mathf.Sin(j * tubeSize);

                // Add the vertex to the vertex array
                vertices[iv1] = new Vector3(x, y, z);

                // "Draw" the first triangle involving this vertex
                triangleIndices[iv1 * 6] = iv1;
                triangleIndices[iv1 * 6 + 1] = iv2;
                triangleIndices[iv1 * 6 + 2] = iv3;
                // Finish the quad
                triangleIndices[iv1 * 6 + 3] = iv3;
                triangleIndices[iv1 * 6 + 4] = iv4;
                triangleIndices[iv1 * 6 + 5] = iv1;
            }
        }
    }

    public void UpdateMesh(Vector3[] vertices, int[] triangleIndices)
    {
        Mesh _mesh = GetMesh();
        //_mesh.vertices = new Vector3[this.vertices];
        if (_mesh.vertices.Length > vertices.Length)
        {
            _mesh.triangles = triangleIndices;
            _mesh.vertices = vertices;
        }
        else
        {
            _mesh.vertices = vertices;
            _mesh.triangles = triangleIndices;
        }

        _mesh.RecalculateBounds();
        _mesh.RecalculateNormals();
        _mesh.Optimize();
        Mf.sharedMesh = _mesh;
    }

    public void UpdateMeshCollider()
    {
        MeshCollider collider = GetComponent<MeshCollider>();
        if (collider == null)
        {
            return;
        }

        collider.sharedMesh = Mf.sharedMesh;
    }

    public void NewMesh()
    {
        Mf.sharedMesh = CreateNewMesh();
        UpdateTorus();
    }
}
