using System;
using System.Collections;
using UnityEngine;

namespace SodaSailor
{
    public class EnemyShip : Ship
    {
        private const float HIDDEN_SNAP_POSITION = 1.2f;
        private const float SHOWN_SNAP_POSITION = .7f;
        private const float TRANSITION_TIME = 2f;

        protected override void Awake()
        {
            base.Awake();
            gameObject.SetActive(false);
        }

        public void ShowUp()
        {
            gameObject.SetActive(true);
            _water.SnapPosition = HIDDEN_SNAP_POSITION;
            StartCoroutine(ShowUpCoroutine());
        }

        private IEnumerator ShowUpCoroutine()
        {
            float time = 0;
            float ratio = time / TRANSITION_TIME;
            while (ratio < 1f)
            {
                time += Time.deltaTime;
                ratio = time / TRANSITION_TIME;
                _water.SnapPosition = Mathf.Lerp(HIDDEN_SNAP_POSITION, SHOWN_SNAP_POSITION, Mathf.SmoothStep(0f, 1f, ratio));
                yield return null;
            }

            InvokeRepeating(nameof(Fire), 0.1f, 5f);
        }

        public void Hide()
        {
            CancelInvoke(nameof(Fire));
            StartCoroutine(HideCoroutine());
        }

        public IEnumerator HideCoroutine()
        {
            float time = 0;
            float ratio = time / TRANSITION_TIME;
            while (ratio < 1f)
            {
                time += Time.deltaTime;
                ratio = time / TRANSITION_TIME;
                _water.SnapPosition = Mathf.Lerp(SHOWN_SNAP_POSITION, HIDDEN_SNAP_POSITION, Mathf.SmoothStep(0f, 1f, ratio));
                yield return null;
            }

            gameObject.SetActive(false);
        }
    }
}