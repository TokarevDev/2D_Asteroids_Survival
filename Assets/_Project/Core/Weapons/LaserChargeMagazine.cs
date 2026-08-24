using System;
using Game.Core.Configuration;

namespace Game.Core.Weapons
{
    public sealed class LaserChargeMagazine
    {
        private readonly float _rechargeDurationSeconds;

        private float _rechargeElapsedSeconds;

        public int CurrentCharges { get; private set; }
        public int MaxCharges { get; }

        public float RechargeRemainingSeconds
        {
            get
            {
                if (CurrentCharges >= MaxCharges)
                {
                    return 0f;
                }

                float remaining = _rechargeDurationSeconds - _rechargeElapsedSeconds;

                return remaining > 0f ? remaining : 0f;
            }
        }

        public LaserChargeMagazine(PlayerConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (config.LaserMaxCharges <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(config.LaserMaxCharges),
                    config.LaserMaxCharges,
                    "Maximum laser charges must be greater than zero");
            }

            if (config.LaserRechargeSeconds <= 0f)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(config.LaserRechargeSeconds),
                    config.LaserRechargeSeconds,
                    "Laser recharge duration must be greater than zero");
            }

            MaxCharges = config.LaserMaxCharges;
            CurrentCharges = config.LaserMaxCharges;
            _rechargeDurationSeconds = config.LaserRechargeSeconds;
        }

        public bool TryConsume()
        {
            if (CurrentCharges == 0)
            {
                return false;
            }

            bool wasFull = CurrentCharges == MaxCharges;
            CurrentCharges--;

            if (wasFull)
            {
                _rechargeElapsedSeconds = 0f;
            }

            return true;
        }

        public void Tick(float deltaTime)
        {
            if (deltaTime < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(deltaTime), deltaTime, "Delta time cannot be negative");
            }

            if (CurrentCharges >= MaxCharges || deltaTime == 0f)
            {
                return;
            }

            _rechargeElapsedSeconds += deltaTime;

            while (CurrentCharges < MaxCharges && _rechargeElapsedSeconds >= _rechargeDurationSeconds)
            {
                _rechargeElapsedSeconds -= _rechargeDurationSeconds;
                CurrentCharges++;
            }

            if (CurrentCharges == MaxCharges)
            {
                _rechargeElapsedSeconds = 0f;
            }
        }
    }
}
