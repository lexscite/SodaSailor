using UnityEngine;

namespace SodaSailor
{
	[RequireComponent(typeof(BoxCollider))]
	public class PickablesSpawner : MonoBehaviour
	{
		protected BoxCollider _collider;
		protected Bounds _bounds;

		private void Awake()
		{
			_collider = GetComponent<BoxCollider>();
			_bounds = _collider.bounds;
		}

        public Pickable Spawn(Pickable prefab)
		{
			var pickable = Instantiate(prefab);
			pickable.name = prefab.name;
			pickable.transform.position = GetRandomPointInCollider();
			pickable.transform.SetParent(transform);

            return pickable;
		}

		private Vector3 GetRandomPointInCollider()
		{
			return new Vector3(
				Random.Range(_bounds.min.x, _bounds.max.x),
				Random.Range(_bounds.min.y, _bounds.max.y),
				Random.Range(_bounds.min.z, _bounds.max.z)
			);
		}
	}

}