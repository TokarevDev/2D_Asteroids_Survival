using System;

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

        public LaserChargeMagazine(int maxCharges, float rechargeDurationSeconds)
        {
            if (maxCharges <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxCharges), maxCharges,
                    "Maximum laser charges must be greater than zero");
            }

            if (rechargeDurationSeconds <= 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(rechargeDurationSeconds), rechargeDurationSeconds,
                    "Laser recharge duration must be greater than zero");
            }

            MaxCharges = maxCharges;
            CurrentCharges = maxCharges;
            _rechargeDurationSeconds = rechargeDurationSeconds;
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
