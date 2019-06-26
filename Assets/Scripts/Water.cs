using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(LineRenderer))]
public class Water : MonoBehaviour
{
    [Header("Mesh")]
    [SerializeField]
    protected float _width = 10f;
    [SerializeField]
    protected float _depth = -10f;
    [SerializeField]
    protected int _polygonsCount = 20;

    [Header("Waves")]
    [SerializeField]
    protected float _waveFrequency = 1;
    [SerializeField]
    protected float _waveHeight = .2f;
    [SerializeField]
    protected float _waveSpeed = .01f;

    [Header("Snapping")]
    [SerializeField]
    protected int _snapPointIndex;
    [SerializeField]
    protected GameObject _snappingObject;
    [SerializeField]
    protected bool _snapRotation;

    protected MeshFilter _meshFilter;
    protected Mesh _mesh;
    protected LineRenderer _lineRenderer;

    protected int _stepCounter;

    protected void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _meshFilter.sharedMesh = new Mesh();
        _mesh = _meshFilter.sharedMesh;
        _lineRenderer = GetComponent<LineRenderer>();
        GenerateMesh();
    }

    protected void Update()
    {
        UpdateMesh();
    }

    protected void UpdateMesh()
    {
        _stepCounter++;
        var vertices = _mesh.vertices;

        for (var i = 0; i < vertices.Length; i++)
        {
            if (System.Math.Abs(vertices[i].y - _depth) > float.Epsilon)
            {
                var x = vertices[i].x;
                vertices[i].y = Mathf.Cos((x + _waveSpeed * _stepCounter) * _waveFrequency) * _waveHeight;
            }
        }

        _mesh.vertices = vertices;
    }

    protected void GenerateMesh()
    {
        var left = -_width / 2;
        int nodecount = _polygonsCount + 1;
        _lineRenderer.positionCount = nodecount;
        _lineRenderer.startWidth = 0.1f;
        _lineRenderer.endWidth = 0.1f;

        var positions = new Vector3[nodecount];

        for (int i = 0; i < nodecount; i++)
        {
            var x = left + _width * i / _polygonsCount;
            var y = Mathf.Cos(x * _waveFrequency) * _waveHeight;
            positions[i] = new Vector3(x, y, transform.position.z);
            _lineRenderer.SetPosition(i, positions[i]);
        }

        Vector3[] vertices = new Vector3[_polygonsCount * 4];
        Vector2[] uvs = new Vector2[_polygonsCount * 4];
        int[] triangles = new int[_polygonsCount * 6];

        for (int i = 0, vi = 0, ti = 0; i < _polygonsCount; i++, vi += 4, ti += 6)
        {
            vertices[vi] = positions[i];
            vertices[vi + 1] = positions[i + 1];
            vertices[vi + 2] = new Vector3(positions[i].x, _depth, positions[i].z);
            vertices[vi + 3] = new Vector3(positions[i + 1].x, _depth, positions[i + 1].z);

            uvs[vi] = new Vector2(1f / _polygonsCount * i, 1);
            uvs[vi + 1] = new Vector2(1f / _polygonsCount * (i + 1), 1);
            uvs[vi + 2] = new Vector2(1f / _polygonsCount * i, 0);
            uvs[vi + 3] = new Vector2(1f / _polygonsCount * (i + 1), 0);

            triangles[ti] = vi;
            triangles[ti + 1] = vi + 1;
            triangles[ti + 2] = vi + 3;
            triangles[ti + 3] = vi + 3;
            triangles[ti + 4] = vi + 2;
            triangles[ti + 5] = vi;
        }

        _mesh.vertices = vertices;
        _mesh.uv = uvs;
        _mesh.triangles = triangles;
    }
}