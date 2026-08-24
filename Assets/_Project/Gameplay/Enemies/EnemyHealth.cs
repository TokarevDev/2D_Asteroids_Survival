using System;
using Game.Gameplay.Combat;

namespace Game.Gameplay.Enemies
{
    public sealed class EnemyHealth : IDamageable
    {
        public event Action<DeathSource> Died;

        private readonly Health _health = new();

        private DeathSource _deathSource;

        public int CurrentHealth => _health.CurrentHealth;
        public bool IsDead => _health.IsDead;

        public EnemyHealth()
        {
            _health.Died += OnDied;
        }

        public void Initialize(int maxHealth)
        {
            _deathSource = DeathSource.Environment;
            _health.Initialize(maxHealth);
        }

        public void TakeDamage(int damage)
        {
            if (damage <= 0 || IsDead)
            {
                return;
            }

            _deathSource = DeathSource.Player;
            _health.TakeDamage(damage);
        }

        public void Kill(DeathSource deathSource)
        {
            if (IsDead)
            {
                return;
            }

            _deathSource = deathSource;
            _health.TakeDamage(CurrentHealth);
        }

        private void OnDied()
        {
            Died?.Invoke(_deathSource);
        }
    }
}
