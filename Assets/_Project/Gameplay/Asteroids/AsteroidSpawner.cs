using System;
using Game.Core.Configuration;
using Game.Core.Enemies;
using Game.Gameplay.Enemies.Spawning;
using Game.Gameplay.Session;
using UnityEngine;
using Zenject;

namespace Game.Gameplay.Asteroids
{
    public sealed class AsteroidSpawner : IInitializable, ITickable
    {
        private const float SecondsPerMinute = 60f;

        private readonly EnemySpawnProcess _spawnProcess;
        private readonly SurvivalTimer _survivalTimer;
        private readonly float _initialSpawnInterval;
        private readonly float _minimumSpawnInterval;
        private readonly float _intervalReductionPerMinute;

        public AsteroidSpawner(EnemyRegistry enemyRegistry, WorldConfig worldConfig, EnemyConfig enemyConfig,
            SurvivalTimer survivalTimer,
            AsteroidSpawnAction spawnAction)
        {
            if (enemyConfig == null)
            {
                throw new ArgumentNullException(nameof(enemyConfig));
            }

            _survivalTimer = survivalTimer ?? throw new ArgumentNullException(nameof(survivalTimer));

            _initialSpawnInterval = enemyConfig.AsteroidSpawnIntervalSeconds;
            _minimumSpawnInterval = enemyConfig.MinimumAsteroidSpawnIntervalSeconds;
            _intervalReductionPerMinute = enemyConfig.AsteroidSpawnIntervalReductionPerMinute;

            ValidateConfiguration();

            _spawnProcess = new EnemySpawnProcess(enemyRegistry, worldConfig, spawnAction, GetCurrentSpawnInterval);
        }

        public void Initialize()
        {
            _spawnProcess.Begin();
        }

        public void Tick()
        {
            _spawnProcess.Advance(Time.deltaTime);
        }

        private float GetCurrentSpawnInterval()
        {
            float elapsedMinutes = _survivalTimer.ElapsedSeconds / SecondsPerMinute;
            float intervalReduction = elapsedMinutes * _intervalReductionPerMinute;

            return Mathf.Max(_minimumSpawnInterval, _initialSpawnInterval - intervalReduction);
        }

        private void ValidateConfiguration()
        {
            if (_minimumSpawnInterval > _initialSpawnInterval)
            {
                throw new InvalidOperationException("Minimum asteroid spawn interval cannot exceed " +
                                                    "the initial interval");
            }
        }
    }
}
