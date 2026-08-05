using System;
using System.Collections.Generic;

namespace Game.Core.Enemies
{
    public sealed class EnemyRegistry
    {
        private readonly List<EnemyEntity> _enemies = new();

        public IReadOnlyList<EnemyEntity> Enemies => _enemies;
        public int Count => _enemies.Count;

        public bool Register(EnemyEntity enemy)
        {
            if (enemy == null)
            {
                throw new ArgumentNullException(nameof(enemy));
            }

            if (_enemies.Contains(enemy))
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

            return _enemies.Remove(enemy);
        }

        public void Clear()
        {
            _enemies.Clear();
        }
    }
}
