using UnityEngine;

namespace SodaSailor
{
    public class DailyRewardPickable : Pickable
    {
        [SerializeField]
        protected ParticleSystem _particleSystemPrefab;

        private GameScreensManager _gameScreensManager;
        private Transform _targetTranform;

        protected override void Awake()
        {
            base.Awake();
            _gameScreensManager = GameObject.Find("GameScreensManager").GetComponent<GameScreensManager>();
            _targetTranform = (_gameScreensManager.GetScreenById("SailScreen") as SailScreen).GoldCounterIcon.transform;
        }

        protected override void PickUp()
        {
            base.PickUp();

            var particles = Instantiate(_particleSystemPrefab).GetComponent<ParticleSystem>();
            var particlesTransform = particles.transform;
            particlesTransform.position = _transform.position;

            var particlesAttractor = particles.GetComponent<ParticleAttractor>();
            particlesAttractor.TargetTransform = _targetTranform;

            particles.Play();

            _playerManager.GiveDailyReward();
        }
    }
}
