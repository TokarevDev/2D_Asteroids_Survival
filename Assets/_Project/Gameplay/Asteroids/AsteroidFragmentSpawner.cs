using System;
using Game.Core.Configuration;
using Game.Core.Enemies;
using UnityEngine;

namespace Game.Gameplay.Asteroids
{
    public sealed class AsteroidFragmentSpawner
    {
        private const float MinimumDirectionSqrMagnitude = 0.0001f;

        private readonly AsteroidPool _asteroidPool;
        private readonly EnemyRegistry _enemyRegistry;
        private readonly IGameConfigProvider _configProvider;

        public AsteroidFragmentSpawner(AsteroidPool asteroidPool, EnemyRegistry enemyRegistry,
            IGameConfigProvider configProvider)
        {
            _asteroidPool = asteroidPool ?? throw new ArgumentNullException(nameof(asteroidPool));

            _enemyRegistry = enemyRegistry ?? throw new ArgumentNullException(nameof(enemyRegistry));

            _configProvider = configProvider ?? throw new ArgumentNullException(nameof(configProvider));
        }

        public int Spawn(Vector2 position, Vector2 parentVelocity)
        {
            EnemyConfig enemyConfig = _configProvider.Enemy;
            EnemyParameters fragmentParameters =
                enemyConfig.GetParameters(EnemyType.Fragment);

            int availableSlots = _configProvider.World.MaxEnemies - _enemyRegistry.Count;

            int fragmentCount = Mathf.Min(enemyConfig.FragmentCount, availableSlots);

            if (fragmentCount <= 0)
            {
                return 0;
            }

            Vector2 baseDirection = parentVelocity.sqrMagnitude >= MinimumDirectionSqrMagnitude
                ? parentVelocity.normalized
                : Vector2.up;

            float angleStep = fragmentCount > 1 ? enemyConfig.FragmentSpreadDegrees / (fragmentCount - 1) : 0f;

            float angle = fragmentCount > 1 ? -enemyConfig.FragmentSpreadDegrees * 0.5f : 0f;

            float fragmentSpeed = fragmentParameters.Speed;

            for (int i = 0; i < fragmentCount; i++)
            {
                Vector2 direction = Rotate(baseDirection, angle);
                Vector2 velocity = direction * fragmentSpeed;
                _asteroidPool.GetFragment(position, velocity);

                angle += angleStep;
            }

            return fragmentCount;
        }

        private static Vector2 Rotate(Vector2 direction, float angleDegrees)
        {
            float angleRadius = angleDegrees * Mathf.Deg2Rad;
            float sin = Mathf.Sin(angleRadius);
            float cos = Mathf.Cos(angleRadius);

            return new Vector2(direction.x * cos - direction.y * sin, direction.x * sin + direction.y * cos);
        }
    }
}
