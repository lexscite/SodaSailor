using System.Collections;
using UnityEngine;

namespace SodaSailor
{
    public abstract class Pickable : TouchableObject
    {
        protected PlayerManager _playerManager;

        protected Transform _transform;

        protected override void OnTouch()
        {
            PickUp();
        }

        protected override void Awake()
        {
            base.Awake();
            _transform = transform;
            _playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        }

        protected virtual void Update()
        {
            _transform.Translate(-1f * Time.deltaTime, 0, 0);

            if (_transform.position.x < -3f)
            {
                Destroy(gameObject);
            }
        }

        protected virtual void PickUp()
        {
            Vanish();
        }

        public void Vanish()
        {
            _collider.enabled = false;
            StartCoroutine(VanishCoroutine(Vector3.one, Vector3.zero, .2f));
        }

        private IEnumerator VanishCoroutine(Vector3 start, Vector3 end, float duration)
        {
            float time = 0;
            float ratio = time / duration;

            while (ratio < 1f)
            {
                time += Time.deltaTime;
                ratio = time / duration;
                _transform.localScale = Vector3.Lerp(start, end, ratio);
                yield return null;
            }

            Destroy(gameObject);
        }
    }
}
