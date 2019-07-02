using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SodaSailor
{
    public class DailyRewardPickable : Pickable
    {
        [SerializeField]
        protected ParticleSystem _pickParticleSystem;

        protected override void PickUp()
        {
            base.PickUp();
            _playerManager.GiveDailyReward();
            var pickParticleSystem = Instantiate(_pickParticleSystem).GetComponent<ParticleSystem>();
            pickParticleSystem.externalForces.SetInfluence(0, _playerManager.Ship.ParticlesMagnet);
            pickParticleSystem.transform.position = transform.position;
        }
    }
}
