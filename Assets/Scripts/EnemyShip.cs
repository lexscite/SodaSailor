using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SodaSailor
{
    public class EnemyShip : MonoBehaviour
    {
        private const float START_SNAP_POSITION = 1.2f;

        [SerializeField]
        protected Water _water;

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        public void ShowUp()
        {
            gameObject.SetActive(true);
            _water.SnapPosition = START_SNAP_POSITION;
            StartCoroutine(ShowUpCoroutine(.7f, 2f));
        }

        private IEnumerator ShowUpCoroutine(float end, float duration)
        {
            float time = 0;
            float ratio = time / duration;
            while (ratio < 1f)
            {
                time += Time.deltaTime;
                ratio = time / duration;
                _water.SnapPosition = Mathf.Lerp(START_SNAP_POSITION, end, Mathf.SmoothStep(0f, 1f, ratio));
                yield return null;
            }
        }
    }
}