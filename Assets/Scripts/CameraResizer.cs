using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class CameraResizer : MonoBehaviour
{
    [Header("Native parameters")]
    [SerializeField]
    protected int _resolutionX = 1125;
    [SerializeField]
    protected int _resolutionY = 2436;

    protected float _orthographicSize;
    protected float _aspectRatio;

    protected Camera _camera;
    protected Transform _camerTranform;

#if UNITY_EDITOR
    private Vector2 _resolution;
#endif //UNITY_EDITOR

    protected void Awake()
    {
#if UNITY_EDITOR
        _resolution = new Vector2(Screen.width, Screen.height);
#endif //UNITY_EDITOR

        _camera = GetComponent<Camera>();
        _camerTranform = _camera.transform;
        _orthographicSize = _camera.orthographicSize;
        _aspectRatio = _resolutionX < _resolutionY ? (float)_resolutionX / _resolutionY : (float)_resolutionY / _resolutionX;

        Resize();
    }

    protected void Update()
    {
#if UNITY_EDITOR
        var resolution = new Vector2(Screen.width, Screen.height);
        if (_resolution != resolution)
        {
            _resolution = resolution;
            Resize();
        }
#endif //UNITY_EDITOR
    }

    private void Resize()
    {
        var aspectRatio = (float)Screen.width / Screen.height;
        var size = _orthographicSize / (aspectRatio / _aspectRatio);

        _camera.orthographicSize = size;
    }
}