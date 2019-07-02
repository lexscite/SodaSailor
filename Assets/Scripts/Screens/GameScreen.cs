using UnityEngine;

namespace SodaSailor
{
    public abstract class GameScreen : MonoBehaviour
    {
        public string Id
        {
            get;
            private set;
        }

        protected virtual void Awake()
        {
            Id = gameObject.name;
        }
    }
}