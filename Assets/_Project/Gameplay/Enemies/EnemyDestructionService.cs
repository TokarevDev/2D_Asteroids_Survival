using System;
using Game.Core.Enemies;
using Game.Gameplay.Combat;

namespace Game.Gameplay.Enemies
{
    public sealed class EnemyDestructionService
    {
        private readonly EnemyDamageableRegistry _damageableRegistry;

        public EnemyDestructionService(EnemyDamageableRegistry damageableRegistry)
        {
            _damageableRegistry = damageableRegistry
                                  ?? throw new ArgumentNullException(nameof(damageableRegistry));
        }

        public void DestroyByPlayer(EnemyEntity enemy)
        {
            if (enemy == null)
            {
                throw new ArgumentNullException(nameof(enemy));
            }

            if (!_damageableRegistry.TryGet(enemy, out IDamageable damageable))
            {
                throw new InvalidOperationException("Enemy entity has no associated damageable");
            }

            damageable.TakeDamage(damageable.CurrentHealth);
        }
    }
}
