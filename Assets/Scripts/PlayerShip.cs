using UnityEngine;

namespace SodaSailor
{
    public class PlayerShip : MonoBehaviour
    {
        [SerializeField]
        protected ParticleSystemForceField _particlesMagnet;

        public ParticleSystemForceField ParticlesMagnet { get { return _particlesMagnet; } }
    }
}