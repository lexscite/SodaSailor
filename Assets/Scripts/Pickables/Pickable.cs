using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SodaSailor
{
    public abstract class Pickable : TouchableObject
    {
        protected PlayerManager _playerManager;

        protected override void OnTouch()
        {
            PickUp();
        }

        protected override void Awake()
        {
            base.Awake();
            _playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        }

        protected virtual void Update()
        {
            transform.Translate(-.5f * Time.deltaTime, 0, 0);

            if (transform.position.x < -3f)
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
                transform.localScale = Vector3.Lerp(start, end, ratio);
                yield return null;
            }

            Destroy(gameObject);
        }
    }
}
