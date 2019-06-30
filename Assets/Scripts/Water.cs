using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Water : MonoBehaviour
{
    [Header("Mesh Generation")]
    [SerializeField]
    protected Canvas _uiCanvas;
    [SerializeField]
    protected int _polygonsCountMultiplier = 2;

    [Header("Waves")]
    [SerializeField]
    [Range(.0001f, 1)]
    protected float _waveFrequency = 1;
    [SerializeField]
    [Range(0, 1)]
    protected float _waveHeight = .2f;
    [SerializeField]
    [Range(1, 1000)]
    protected int _steps = 10;
    [SerializeField]
    protected bool _movingMaterial;

    [Header("Snapping")]
    [SerializeField]
    protected GameObject _snapTarget;
    [SerializeField]
    [Range(0, 1)]
    protected float _snapPosition;
    [SerializeField]
    protected float _yOffset;

    protected MeshFilter _meshFilter;
    protected MeshRenderer _meshRenderer;
    protected Mesh _mesh;

    protected int _stepCounter;

    protected float _left;
    protected float _width;
    protected float _bottom;
    protected float _step;

    protected List<int> _surfaceIndices = new List<int>();
    protected List<int> _bottomIndices = new List<int>();

#if UNITY_EDITOR
    private Vector2 _resolution;
#endif //UNITY_EDITOR

    protected void Awake()
    {
#if UNITY_EDITOR
        _resolution = new Vector2(Screen.width, Screen.height);
#endif //UNITY_EDITOR

        _meshFilter = GetComponent<MeshFilter>();
        _meshFilter.sharedMesh = new Mesh();
        _mesh = _meshFilter.sharedMesh;
        _mesh.name = "Water";
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    protected void Start()
    {
        GenerateMesh();
    }

    protected void Update()
    {
#if UNITY_EDITOR
        var resolution = new Vector2(Screen.width, Screen.height);
        if (_resolution != resolution)
        {
            _resolution = resolution;
            GenerateMesh();
        }
#endif //UNITY_EDITOR

        UpdateMesh();

        if (_snapTarget)
        {
            UpdateSnapTarget();
        }
    }

    protected void UpdateMesh()
    {
        _stepCounter++;
        _step = 2 * Mathf.PI / _waveFrequency / _steps;

        var vertices = _mesh.vertices;

        foreach (int i in _surfaceIndices)
        {
            vertices[i].y = GetY(vertices[i].x);
        }

        _mesh.vertices = vertices;

        if (_movingMaterial)
        {
            var textureOffset = new Vector2(1.0f / _steps * _stepCounter, 0);
            _meshRenderer.material.mainTextureOffset = textureOffset;
        }

        if (_stepCounter == _steps)
        {
            _stepCounter = 0;
        }
    }

    protected void UpdateSnapTarget()
    {
        var targetTransform = _snapTarget.transform;
        var targetPosition = targetTransform.position;
        targetPosition.x = _left + _width * _snapPosition;
        targetPosition.y = GetY(targetPosition.x) + transform.position.y + _yOffset;
        targetTransform.position = targetPosition;
    }

    protected float GetY(float x)
    {
        return Mathf.Cos((x + _step * _stepCounter) * _waveFrequency) * _waveHeight;
    }

    protected void GenerateMesh()
    {
        var uiCanvasRectTransform = _uiCanvas.GetComponent<RectTransform>();
        Vector3[] uiCanvasCorners = new Vector3[4];
        uiCanvasRectTransform.GetWorldCorners(uiCanvasCorners);

        _left = uiCanvasCorners[0].x;
        _width = uiCanvasCorners[3].x - _left;
        _bottom = uiCanvasCorners[0].y - transform.position.y;

        var polygonsCount = Mathf.FloorToInt(_width * _polygonsCountMultiplier);
        var nodecount = polygonsCount + 1;
        var positions = new Vector3[nodecount];

        for (int i = 0; i < nodecount; i++)
        {
            var x = _left + _width * i / polygonsCount;
            var y = Mathf.Cos(x * _waveFrequency) * _waveHeight;
            positions[i] = new Vector3(x, y, 0);
        }

        Vector3[] vertices = new Vector3[polygonsCount * 4];
        Vector2[] uvs = new Vector2[polygonsCount * 4];
        int[] triangles = new int[polygonsCount * 6];

        _mesh.Clear();
        _surfaceIndices.Clear();
        _bottomIndices.Clear();

        for (int i = 0, vi = 0, ti = 0; i < polygonsCount; i++, vi += 4, ti += 6)
        {
            _surfaceIndices.Add(vi);
            _surfaceIndices.Add(vi + 1);
            _bottomIndices.Add(vi + 2);
            _bottomIndices.Add(vi + 3);

            vertices[vi] = positions[i];
            vertices[vi + 1] = positions[i + 1];
            vertices[vi + 2] = new Vector3(positions[i].x, _bottom, positions[i].z);
            vertices[vi + 3] = new Vector3(positions[i + 1].x, _bottom, positions[i + 1].z);

            uvs[vi] = new Vector2(1f / polygonsCount * i, 1);
            uvs[vi + 1] = new Vector2(1f / polygonsCount * (i + 1), 1);
            uvs[vi + 2] = new Vector2(1f / polygonsCount * i, 0);
            uvs[vi + 3] = new Vector2(1f / polygonsCount * (i + 1), 0);

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