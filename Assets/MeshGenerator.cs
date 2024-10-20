using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    public bool ShowGizmos;
    [Range(0, 1)]
    public float GizmoTransparency;

    Mesh _mesh;

    Vector3[] _vertices;
    int[] _triangles;

    public int xSize = 20;
    public int zSize = 20;

    void Start() {
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;

        CreateShape();
        UpdateMesh();
    }

    void Update() {
        
    }

    void CreateShape() {
        _vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        for(int i = 0, z = 0; z <= zSize; z++) {
            for(int x = 0; x <= xSize; x++) {
                float y = Mathf.PerlinNoise(x * .3f, z * .3f) * 2f;
                _vertices[i] = new Vector3(x, y, z);
                i++;
            }
        }

        _triangles = new int[xSize * zSize * 6];
        int vert = 0;
        int tris = 0;

        for(int z = 0; z < zSize; z++) {
            for(int x = 0; x < xSize; x++) {
                _triangles[tris + 0] = vert + 0;
                _triangles[tris + 1] = vert + xSize + 1;
                _triangles[tris + 2] = vert + 1;
                _triangles[tris + 3] = vert + 1;
                _triangles[tris + 4] = vert + xSize + 1;
                _triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;

            }
            vert++;
        }
    }

    void UpdateMesh() {
        _mesh.Clear();

        _mesh.vertices = _vertices;
        _mesh.triangles = _triangles;

        _mesh.RecalculateNormals();
    }

    void OnDrawGizmos() {
        if(_vertices == null || !ShowGizmos) {
            return;
        }

        for(int i = 0; i < _vertices.Length; i++) {
            Gizmos.color = new Color(Color.magenta.r, Color.magenta.g, Color.magenta.b, GizmoTransparency);
            Gizmos.DrawSphere(_vertices[i], .1f);
        }
    }
}
