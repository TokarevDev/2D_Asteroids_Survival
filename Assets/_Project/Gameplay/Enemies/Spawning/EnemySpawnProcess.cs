using System;
using Game.Core.Configuration;
using Game.Core.Enemies;

namespace Game.Gameplay.Enemies.Spawning
{
    public sealed class EnemySpawnProcess
    {
        private readonly EnemyRegistry _enemyRegistry;
        private readonly IEnemySpawnAction _spawnAction;
        private readonly Func<float> _spawnIntervalProvider;
        private readonly int _maxEnemies;

        private float _timeUntilNextSpawn;
        private bool _isInitialized;

        public EnemySpawnProcess(EnemyRegistry enemyRegistry, WorldConfig worldConfig, IEnemySpawnAction spawnAction,
            Func<float> spawnIntervalProvider)
        {
            _enemyRegistry = enemyRegistry ?? throw new ArgumentNullException(nameof(enemyRegistry));

            if (worldConfig == null)
            {
                throw new ArgumentNullException(nameof(worldConfig));
            }

            _spawnAction = spawnAction ?? throw new ArgumentNullException(nameof(spawnAction));

            _spawnIntervalProvider =
                spawnIntervalProvider ?? throw new ArgumentNullException(nameof(spawnIntervalProvider));

            _maxEnemies = worldConfig.MaxEnemies;
        }

        public void Begin()
        {
            if (_isInitialized)
            {
                throw new InvalidOperationException("Enemy spawn process is already initialized");
            }

            _isInitialized = true;

            TrySpawn();
            ResetTimer();
        }

        public void Advance(float deltaTime)
        {
            if (!_isInitialized)
            {
                throw new InvalidOperationException("Enemy spawn process is not initialized");
            }

            if (deltaTime < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(deltaTime), deltaTime, "Delta time cannot be negative");
            }

            _timeUntilNextSpawn -= deltaTime;

            if (_timeUntilNextSpawn > 0f)
            {
                return;
            }

            TrySpawn();
            ResetTimer();
        }

        private void TrySpawn()
        {
            if (_enemyRegistry.Count >= _maxEnemies)
            {
                return;
            }

            _spawnAction.Spawn();
        }

        private void ResetTimer()
        {
            float spawnInterval = _spawnIntervalProvider();

            if (spawnInterval <= 0f)
            {
                throw new InvalidOperationException("Enemy spawn interval must be greater than zero");
            }

            _timeUntilNextSpawn = spawnInterval;
        }
    }
}
