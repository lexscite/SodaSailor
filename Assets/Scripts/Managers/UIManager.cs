using UnityEngine;

namespace SodaSailor
{
	public class UIManager : MonoBehaviour
	{
		private Camera _camera;

		private bool isTouching;

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Update()
		{
			isTouching = false;

			Vector2 position = Vector2.zero;

			if (Input.touchCount > 0)
			{
				if (Input.GetTouch(0).phase.Equals(TouchPhase.Began))
				{
					isTouching = true;
					position = Input.GetTouch(0).position;
				}
			}
			else
			{
				if (Input.GetMouseButtonDown(0))
				{
					isTouching = true;
					position = Input.mousePosition;
				}
			}

			if (isTouching)
			{
				ProcessInput(position);
			}
		}

		private void ProcessInput(Vector2 position)
		{
			Ray ray = _camera.ScreenPointToRay(position);
			if (Physics.Raycast(ray, out RaycastHit hit))
			{
				var touchable = hit.transform.gameObject.GetComponent<TouchableObject>();
				touchable?.SendMessage("OnTouch");
			}
		}
	}
}