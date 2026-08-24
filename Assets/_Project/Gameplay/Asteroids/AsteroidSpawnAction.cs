using System;
using Game.Core.Configuration;
using Game.Core.Enemies;
using Game.Gameplay.Enemies.Spawning;
using Game.Gameplay.World;
using UnityEngine;

namespace Game.Gameplay.Asteroids
{
    public sealed class AsteroidSpawnAction : IEnemySpawnAction
    {
        private readonly AsteroidPool _asteroidPool;
        private readonly AsteroidConfigSelector _configSelector;
        private readonly RandomWorldSpawnPointProvider _spawnPointProvider;
        private readonly float _speed;
        private readonly int _maxHealth;

        public AsteroidSpawnAction(AsteroidPool asteroidPool, AsteroidConfigSelector configSelector,
            EnemyConfig enemyConfig, RandomWorldSpawnPointProvider spawnPointProvider)
        {
            _asteroidPool = asteroidPool ?? throw new ArgumentNullException(nameof(asteroidPool));

            _configSelector = configSelector ?? throw new ArgumentNullException(nameof(configSelector));

            if (enemyConfig == null)
            {
                throw new ArgumentNullException(nameof(enemyConfig));
            }

            _spawnPointProvider = spawnPointProvider ?? throw new ArgumentNullException(nameof(spawnPointProvider));

            EnemyParameters parameters = enemyConfig.GetParameters(EnemyType.LargeAsteroid);

            _speed = parameters.Speed;
            _maxHealth = parameters.MaxHealth;
        }

        public void Spawn()
        {
            Vector2 spawnPosition = _spawnPointProvider.GetSpawnPosition();
            Vector2 targetPosition = _spawnPointProvider.GetTargetPosition();
            Vector2 direction = (targetPosition - spawnPosition).normalized;
            Vector2 velocity = direction * _speed;

            AsteroidConfig config = _configSelector.GetNextConfig();

            _asteroidPool.Get(config, EnemyType.LargeAsteroid, spawnPosition, velocity, _maxHealth);
        }
    }
}
