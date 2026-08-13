using System;
using System.Collections.Generic;
using Game.Core.Configuration;
using Game.Core.Enemies;
using Game.Gameplay.Signals;
using Zenject;

namespace Game.Gameplay.Score
{
    public sealed class EnemyRewardService : IInitializable, IDisposable
    {
        private readonly Dictionary<EnemyType, int> _rewardByEnemyType;

        private readonly SignalBus _signalBus;
        private readonly ScoreCounter _scoreCounter;

        public EnemyRewardService(SignalBus signalBus, ScoreCounter scoreCounter,
            IGameConfigProvider configProvider)
        {
            _signalBus = signalBus ?? throw new ArgumentNullException(nameof(signalBus));
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
            _signalBus.Subscribe<EnemyDiedSignal>(OnEnemyDied);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<EnemyDiedSignal>(OnEnemyDied);
        }

        private void OnEnemyDied(EnemyDiedSignal signal)
        {
            if (signal.DeathSource != DeathSource.Player)
            {
                return;
            }

            if (!_rewardByEnemyType.TryGetValue(signal.EnemyType, out int reward))
            {
                throw new InvalidOperationException($"Reward is not configured for enemy type {signal.EnemyType}");
            }

            _scoreCounter.AddScore(reward);
        }
    }
}
