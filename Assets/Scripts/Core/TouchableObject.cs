using UnityEngine;

namespace SodaSailor
{
    [RequireComponent(typeof(BoxCollider))]
    public abstract class TouchableObject : MonoBehaviour
    {
        protected BoxCollider _collider;

        protected virtual void Awake()
        {
            _collider = GetComponent<BoxCollider>();
        }

        protected virtual void OnTouch()
        {

        }
    }

}