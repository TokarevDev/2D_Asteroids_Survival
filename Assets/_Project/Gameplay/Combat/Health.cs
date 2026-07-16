using System;

namespace Game.Gameplay
{
    public sealed class Health
    {
        public event Action<int, int> Changed;
        public event Action Died;

        public int MaxHealth { get; private set; }
        public int CurrentHealth { get; private set; }

        public bool IsDead => CurrentHealth <= 0;

        public void Initialize(int maxHealth)
        {
            if (maxHealth <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxHealth));
            }

            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;

            OnChanged();
        }

        public void TakeDamage(int damage)
        {
            if (damage <= 0 || IsDead)
                return;

            CurrentHealth = Math.Max(0, CurrentHealth - damage);

            OnChanged();

            if (IsDead)
                OnDied();
        }

        private void OnChanged()
        {
            Changed?.Invoke(CurrentHealth, MaxHealth);
        }

        private void OnDied()
        {
            Died?.Invoke();
        }
    }
}
