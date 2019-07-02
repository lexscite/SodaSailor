using System;
using System.Collections;
using UnityEngine;

namespace SodaSailor
{
    public abstract class Ship : MonoBehaviour
    {
        private const int MAX_HEALTH = 100;

        [SerializeField]
        protected Ship _targetShip;
        [SerializeField]
        protected GameObject _cannonballPrefab;
        [SerializeField]
        [Range(-1, 1)]
        protected float _cannonballYOffset;
        [SerializeField]
        protected Water _water;

        private GameObject _cannonball;

        protected Transform _transform;
        private Transform _enemyTransform;
        private Transform _cannonballTransform;

        public event Action OnHealthValueChanged;

        private int _health = MAX_HEALTH;

        public int Health
        {
            get { return _health; }
            private set
            {
                _health = Mathf.Clamp(value, 0, MAX_HEALTH);
                OnHealthValueChanged?.Invoke();
            }
        }

        public void ReceiveDamage(int value)
        {
            Health -= value;
            StartCoroutine(WaterCoroutine(_water.WaveHeight, _water.WaveHeight * 4, 4f));
        }

        public IEnumerator WaterCoroutine(float start, float end, float duration)
        {
            float time = 0;
            float ratio = time / duration;

            while (ratio < 1f)
            {
                time += Time.deltaTime;
                ratio = time / duration;

                _water.WaveHeight = Mathf.Lerp(start, end, ratio);
                yield return null;
            }

            StartCoroutine(WaterCoroutine(end, start, duration));
        }

        protected virtual void Awake()
        {
            _transform = transform;
            _enemyTransform = _targetShip.transform;
        }

        public void Fire()
        {
            if (!_cannonball)
            {
                _cannonball = Instantiate(_cannonballPrefab);
                _cannonball.name = _cannonballPrefab.name;
                _cannonballTransform = _cannonball.transform;
                _cannonballTransform.SetParent(_transform);
                StartCoroutine(FireCoroutine(_transform.position + new Vector3(0, .3f, 0), _enemyTransform.position + new Vector3(0, .3f, 0), 1f));
            }
        }

        private IEnumerator FireCoroutine(Vector3 start, Vector3 end, float duration)
        {
            float time = 0;
            float ratio = time / duration;

            while (ratio < 1f)
            {
                time += Time.deltaTime;
                ratio = time / duration;

                var dir = (end - start).normalized;
                var midPoint = (start + end) / 2f;
                var cross = Vector3.Cross(dir, Vector3.right);
                cross.y = Mathf.Abs(cross.y) + _cannonballYOffset;
                var top = midPoint + cross * 1.5f;

                Vector3 v1 = Vector3.Lerp(start, top, ratio);
                Vector3 v2 = Vector3.Lerp(top, end, ratio);

                _cannonballTransform.position = Vector3.Lerp(v1, v2, ratio);
                yield return null;
            }

            _targetShip.ReceiveDamage(5);
            Destroy(_cannonball.gameObject);
        }
    }
}