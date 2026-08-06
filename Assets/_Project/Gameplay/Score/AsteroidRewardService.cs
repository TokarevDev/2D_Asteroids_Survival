using System;
using System.Collections.Generic;
using Game.Core.Configuration;
using Game.Core.Enemies;
using Zenject;

namespace Game.Gameplay
{
    public sealed class AsteroidRewardService : IInitializable, IDisposable
    {
        private readonly Dictionary<EnemyType, int> _rewardByEnemyType;

        private readonly AsteroidPool _asteroidPool;
        private readonly ScoreCounter _scoreCounter;

        public AsteroidRewardService(AsteroidPool asteroidPool, ScoreCounter scoreCounter,
            IGameConfigProvider configProvider)
        {
            _asteroidPool = asteroidPool ?? throw new ArgumentNullException(nameof(asteroidPool));
            _scoreCounter = scoreCounter ?? throw new ArgumentNullException(nameof(scoreCounter));

            if (configProvider == null)
            {
                throw new ArgumentNullException(nameof(configProvider));
            }

            EnemyConfig enemyConfig = configProvider.Enemy;

            _rewardByEnemyType = new Dictionary<EnemyType, int>(3)
            {
                [EnemyType.LargeAsteroid] = enemyConfig.LargeAsteroid.ScoreReward,
                [EnemyType.Fragment] = enemyConfig.Fragment.ScoreReward,
                [EnemyType.Ufo] = enemyConfig.Ufo.ScoreReward
            };
        }

        public void Initialize()
        {
            _asteroidPool.AsteroidDied += OnAsteroidDied;
        }

        public void Dispose()
        {
            _asteroidPool.AsteroidDied -= OnAsteroidDied;
        }

        private void OnAsteroidDied(EnemyType enemyType, DeathSource deathSource)
        {
            if (deathSource != DeathSource.Player)
            {
                return;
            }

            if (!_rewardByEnemyType.TryGetValue(enemyType, out int reward))
            {
                throw new InvalidOperationException($"Reward is not configured for enemy type {enemyType}");
            }

            _scoreCounter.AddScore(reward);
        }
    }
}
