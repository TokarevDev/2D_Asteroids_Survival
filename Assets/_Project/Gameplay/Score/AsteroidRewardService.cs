using System;
using Zenject;

namespace Game.Gameplay
{
    public sealed class AsteroidRewardService : IInitializable, IDisposable
    {
        private readonly AsteroidPool _asteroidPool;
        private readonly ScoreCounter _scoreCounter;

        public AsteroidRewardService(AsteroidPool asteroidPool, ScoreCounter scoreCounter)
        {
            _asteroidPool = asteroidPool;
            _scoreCounter = scoreCounter;
        }

        public void Initialize()
        {
            _asteroidPool.AsteroidDied += OnAsteroidDied;
        }

        public void Dispose()
        {
            _asteroidPool.AsteroidDied -= OnAsteroidDied;
        }

        private void OnAsteroidDied(Asteroid asteroid, DeathSource deathSource)
        {
            if (deathSource != DeathSource.Player)
            {
                return;
            }

            _scoreCounter.AddScore(asteroid.ScoreReward);
        }
    }
}
