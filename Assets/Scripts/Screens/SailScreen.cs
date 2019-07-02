using UnityEngine;
using UnityEngine.UI;

namespace SodaSailor
{
    public class SailScreen : GameScreen
    {
        [SerializeField]
        protected Text _goldCounterText;
        [SerializeField]
        protected Image _goldCointerIcon;
        [SerializeField]
        protected PickablesSpawner _pickablesSpawner;
        [SerializeField]
        protected DailyRewardPickable _dailyRewardPickablePrefab;
        [SerializeField]
        protected Button _switchStateButton;
        [SerializeField]
        protected EnemyShip _enemyShip;

        private PlayerManager _playerManager;
        private DailyRewardPickable _currentAliveDailyRewardPickable;

        public Image GoldCounterIcon
        {
            get { return _goldCointerIcon; }
        }

        protected override void Awake()
        {
            base.Awake();
            _playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        }

        private void Start()
        {
            InvokeRepeating(nameof(SpawnDailyRewardPickable), 2, 2);
        }

        private void OnEnable()
        {
            RefreshGoldCounter();
            _playerManager.OnGoldValueChanged += RefreshGoldCounter;
            _playerManager.OnDailyRewardReceived += StopDailyRewardSpawning;
            _switchStateButton.onClick.AddListener(SwitchToBattleState);
        }

        private void OnDisable()
        {
            _playerManager.OnGoldValueChanged -= RefreshGoldCounter;
            _playerManager.OnDailyRewardReceived -= StopDailyRewardSpawning;
            _switchStateButton.onClick.RemoveListener(SwitchToBattleState);
        }

        private void SwitchToBattleState()
        {
            _enemyShip.ShowUp();
            StopDailyRewardSpawning();
            _currentAliveDailyRewardPickable?.Vanish();
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