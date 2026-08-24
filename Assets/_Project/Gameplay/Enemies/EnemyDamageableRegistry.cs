using System;
using System.Collections.Generic;
using Game.Core.Enemies;
using Game.Gameplay.Combat;

namespace Game.Gameplay.Enemies
{
    public sealed class EnemyDamageableRegistry
    {
        private readonly Dictionary<EnemyEntity, IDamageable> _damageables = new();

        public bool Register(EnemyEntity enemy, IDamageable damageable)
        {
            if (enemy == null)
            {
                throw new ArgumentNullException(nameof(enemy));
            }

            if (damageable == null)
            {
                throw new ArgumentNullException(nameof(damageable));
            }

            return _damageables.TryAdd(enemy, damageable);
        }

        public bool Unregister(EnemyEntity enemy)
        {
            if (enemy == null)
            {
                throw new ArgumentNullException(nameof(enemy));
            }

            return _damageables.Remove(enemy);
        }

        public bool TryGet(
            EnemyEntity enemy,
            out IDamageable damageable)
        {
            if (enemy == null)
            {
                throw new ArgumentNullException(nameof(enemy));
            }

            return _damageables.TryGetValue(enemy, out damageable);
        }

        public void Clear()
        {
            _damageables.Clear();
        }
    }
}
