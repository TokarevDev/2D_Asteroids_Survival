using System;
using System.Collections.Generic;

namespace Game.Core.Enemies
{
    public sealed class EnemyRegistry
    {
        private readonly List<EnemyEntity> _enemies = new();
        private readonly HashSet<EnemyEntity> _registeredEnemies = new HashSet<EnemyEntity>();

        public IReadOnlyList<EnemyEntity> Enemies => _enemies;
        public int Count => _enemies.Count;

        public bool Register(EnemyEntity enemy)
        {
            if (enemy == null)
            {
                throw new ArgumentNullException(nameof(enemy));
            }

            if (!_registeredEnemies.Add(enemy))
            {
                return false;
            }

            _enemies.Add(enemy);
            return true;
        }

        public bool Unregister(EnemyEntity enemy)
        {
            if (enemy == null)
            {
                throw new ArgumentNullException(nameof(enemy));
            }

            if (!_registeredEnemies.Remove(enemy))
            {
                return false;
            }

            _enemies.Remove(enemy);
            return true;
        }

        public void Clear()
        {
            _registeredEnemies.Clear();
            _enemies.Clear();
        }
    }
}
