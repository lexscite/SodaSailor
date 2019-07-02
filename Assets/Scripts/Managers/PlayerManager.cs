using System;
using UnityEngine;

namespace SodaSailor
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField]
        protected PlayerShip _ship;

        public PlayerShip Ship { get { return _ship; } }

        public event Action OnGoldValueChanged;
        public event Action OnDailyRewardReceived;

        private int _gold;

        public int Gold
        {
            get
            {
                return _gold;
            }
            private set
            {
                _gold = value;
                OnGoldValueChanged?.Invoke();
            }
        }

        public void AddGold(int value)
        {
            Gold += value;
        }

        private bool _isDailyRewardReceived;

        public bool IsDailyRewardReeieved
        {
            get;
            private set;
        }

        public void GiveDailyReward()
        {
            IsDailyRewardReeieved = true;
            AddGold(100);
            OnDailyRewardReceived?.Invoke();
        }
    }

}