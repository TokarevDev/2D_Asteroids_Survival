using System;
using Game.Core.Weapons;
using UnityEngine;
using Zenject;

namespace Game.Gameplay.Weapons
{
    public sealed class LaserRechargeController : ITickable
    {
        private readonly LaserChargeMagazine _chargeMagazine;

        public LaserRechargeController(LaserChargeMagazine chargeMagazine)
        {
            _chargeMagazine = chargeMagazine ?? throw new ArgumentNullException(nameof(chargeMagazine));
        }

        public void Tick()
        {
            _chargeMagazine.Tick(Time.deltaTime);
        }
    }
}
