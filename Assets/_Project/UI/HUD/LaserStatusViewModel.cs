using System;
using Game.Core.Weapons;
using UnityEngine;
using Zenject;

namespace Game.UI
{
    public sealed class LaserStatusViewModel : ITickable
    {
        public event Action<int, int, float> LaserStatusChanged;

        private readonly LaserChargeMagazine _chargeMagazine;

        public int CurrentCharges { get; private set; }
        public int MaxCharges { get; }
        public float RechargeRemainingSeconds { get; private set; }

        public LaserStatusViewModel(LaserChargeMagazine chargeMagazine)
        {
            _chargeMagazine = chargeMagazine ?? throw new ArgumentNullException(nameof(chargeMagazine));

            CurrentCharges = _chargeMagazine.CurrentCharges;
            MaxCharges = _chargeMagazine.MaxCharges;
            RechargeRemainingSeconds = _chargeMagazine.RechargeRemainingSeconds;
        }

        public void Tick()
        {
            int currentCharges = _chargeMagazine.CurrentCharges;
            float rechargeRemainingSeconds = _chargeMagazine.RechargeRemainingSeconds;

            if (currentCharges == CurrentCharges &&
                Mathf.Approximately(rechargeRemainingSeconds, RechargeRemainingSeconds))
            {
                return;
            }

            CurrentCharges = currentCharges;
            RechargeRemainingSeconds = rechargeRemainingSeconds;

            LaserStatusChanged?.Invoke(CurrentCharges, MaxCharges, RechargeRemainingSeconds);
        }
    }
}
