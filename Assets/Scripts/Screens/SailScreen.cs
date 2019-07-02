using UnityEngine;
using UnityEngine.UI;

namespace SodaSailor
{
    public class SailScreen : GameScreen
    {
        private enum SailScreenState
        {
            Sail = 0,
            Battle = 1,
        }

        [SerializeField]
        protected Text _goldCounterText;
        [SerializeField]
        protected Image _goldCointerIcon;
        [SerializeField]
        protected Text _healthText;
        [SerializeField]
        protected PickablesSpawner _pickablesSpawner;
        [SerializeField]
        protected DailyRewardPickable _dailyRewardPickablePrefab;
        [SerializeField]
        protected Button _switchStateButton;
        [SerializeField]
        protected Button _fireButton;
        [SerializeField]
        protected EnemyShip _enemyShip;

        private SailScreenState _currentState = SailScreenState.Sail;

        private PlayerManager _playerManager;
        private PlayerShip _playerShip;

        private DailyRewardPickable _currentAliveDailyRewardPickable;

        public Image GoldCounterIcon
        {
            get { return _goldCointerIcon; }
        }

        protected override void Awake()
        {
            base.Awake();
            _playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
            _playerShip = _playerManager.Ship;
        }

        private void Start()
        {
            InvokeRepeating(nameof(SpawnDailyRewardPickable), 2, 2);
        }

        private void OnEnable()
        {
            RefreshGoldCounter();
            RefreshHealth();

            _playerManager.OnGoldValueChanged += RefreshGoldCounter;
            _playerManager.OnDailyRewardReceived += StopDailyRewardSpawning;
            _playerShip.OnHealthValueChanged += RefreshHealth;
            _switchStateButton.onClick.AddListener(SwitchState);
            _fireButton.onClick.AddListener(_playerManager.Ship.Fire);
        }

        private void OnDisable()
        {
            _playerManager.OnGoldValueChanged -= RefreshGoldCounter;
            _playerManager.OnDailyRewardReceived -= StopDailyRewardSpawning;
            _playerShip.OnHealthValueChanged -= RefreshHealth;
            _switchStateButton.onClick.RemoveListener(SwitchState);
            _fireButton.onClick.RemoveListener(_playerManager.Ship.Fire);
        }

        private void SwitchState()
        {
            if (_currentState == SailScreenState.Sail)
            {
                _enemyShip.ShowUp();
                StopDailyRewardSpawning();
                _currentAliveDailyRewardPickable?.Vanish();
                _currentState = SailScreenState.Battle;
            }
            else
            {
                _enemyShip.Hide();
                InvokeRepeating(nameof(SpawnDailyRewardPickable), 2, 2);
                _currentState = SailScreenState.Sail;
            }
        }

        private void RefreshHealth()
        {
            _healthText.text = _playerShip.Health.ToString();
        }

        private void RefreshGoldCounter()
        {
            _goldCounterText.text = _playerManager.Gold.ToString();
        }

        private void StopDailyRewardSpawning()
        {
            CancelInvoke(nameof(SpawnDailyRewardPickable));
        }

        private void SpawnDailyRewardPickable()
        {
            if (!_currentAliveDailyRewardPickable)
            {
                var pickable = _pickablesSpawner.Spawn(_dailyRewardPickablePrefab);
                _currentAliveDailyRewardPickable = pickable as DailyRewardPickable;
            }
        }
    }

}