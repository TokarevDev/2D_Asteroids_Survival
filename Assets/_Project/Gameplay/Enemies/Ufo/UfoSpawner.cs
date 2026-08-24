using System;
using Game.Core.Configuration;
using Game.Core.Enemies;
using Game.Gameplay.Enemies.Spawning;
using UnityEngine;
using Zenject;

namespace Game.Gameplay.Enemies.Ufo
{
    public sealed class UfoSpawner : IInitializable, ITickable
    {
        private readonly EnemySpawnProcess _spawnProcess;
        private readonly float _spawnInterval;

        public UfoSpawner(EnemyRegistry enemyRegistry, WorldConfig worldConfig, EnemyConfig enemyConfig,
            UfoSpawnAction spawnAction)
        {
            if (enemyConfig == null)
            {
                throw new ArgumentNullException(nameof(enemyConfig));
            }

            _spawnInterval = enemyConfig.UfoSpawnIntervalSeconds;

            _spawnProcess = new EnemySpawnProcess(
                enemyRegistry,
                worldConfig,
                spawnAction,
                GetSpawnInterval);
        }

        public void Initialize()
        {
            _spawnProcess.Begin();
        }

        public void Tick()
        {
            _spawnProcess.Advance(Time.deltaTime);
        }

        private float GetSpawnInterval()
        {
            return _spawnInterval;
        }
    }
}
