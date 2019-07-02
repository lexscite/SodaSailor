using UnityEngine;

namespace SodaSailor
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class CameraResizer : MonoBehaviour
    {
        [SerializeField]
        protected int _width = 1125;
        [SerializeField]
        protected int _height = 2436;
        [SerializeField]
        protected float _size = 5;

        private Camera _camera;

        private float _aspectRatio;

#if UNITY_EDITOR
        private Vector2 _resolution;
#endif //UNITY_EDITOR

        protected void Awake()
        {
#if UNITY_EDITOR
            _resolution = new Vector2(Screen.width, Screen.height);
#endif //UNITY_EDITOR

            _camera = GetComponent<Camera>();
            _aspectRatio = _width < _height ? (float)_width / _height : (float)_height / _width;

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
            var size = _size / (aspectRatio / _aspectRatio);

            _camera.orthographicSize = size;
        }
    }
}