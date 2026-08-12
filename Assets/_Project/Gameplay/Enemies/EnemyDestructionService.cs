using System;
using Game.Core.Enemies;
using Game.Gameplay.Enemies.Ufo;
using UfoEnemy = Game.Gameplay.Enemies.Ufo.Ufo;

namespace Game.Gameplay.Enemies
{
    public sealed class EnemyDestructionService
    {
        private readonly AsteroidPool _asteroidPool;
        private readonly UfoPool _ufoPool;

        public EnemyDestructionService(AsteroidPool asteroidPool, UfoPool ufoPool)
        {
            _asteroidPool = asteroidPool ?? throw new ArgumentNullException(nameof(asteroidPool));

            _ufoPool = ufoPool ?? throw new ArgumentNullException(nameof(ufoPool));
        }

        public void DestroyByPlayer(EnemyEntity enemy)
        {
            if (enemy == null)
            {
                throw new ArgumentNullException(nameof(enemy));
            }

            if (_asteroidPool.TryGetByEntity(enemy, out Asteroid asteroid))
            {
                asteroid.TakeDamage(asteroid.CurrentHealth);
                return;
            }

            if (_ufoPool.TryGetByEntity(enemy, out UfoEnemy ufo))
            {
                ufo.TakeDamage(ufo.CurrentHealth);
                return;
            }

            throw new InvalidOperationException("Enemy entity has no associated visual");
        }
    }
}
