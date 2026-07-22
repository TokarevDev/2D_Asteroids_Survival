using System;
using Zenject;

namespace Game.Gameplay
{
    public sealed class ScoreCounter : IInitializable, IDisposable
    {
        private readonly AsteroidPool _asteroidPool;

        public event Action<int> ScoreChanged;

        public int Score { get; private set; }

        public ScoreCounter(AsteroidPool asteroidPool)
        {
            _asteroidPool = asteroidPool;
        }

        public void Initialize()
        {
            Score = 0;
            _asteroidPool.AsteroidDestroyedByPlayer += AddScore;
        }

        public void Dispose()
        {
            _asteroidPool.AsteroidDestroyedByPlayer -= AddScore;
        }

        private void AddScore(int scoreReward)
        {
            if (scoreReward <= 0)
            {
                return;
            }

            Score += scoreReward;
            ScoreChanged?.Invoke(Score);
        }
    }
}
