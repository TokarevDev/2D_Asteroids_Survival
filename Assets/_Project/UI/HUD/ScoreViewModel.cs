using System;
using Game.Gameplay;
using Zenject;

namespace Game.UI
{
    public sealed class ScoreViewModel : IInitializable, IDisposable
    {
        private readonly ScoreCounter _scoreCounter;

        public event Action<int> ScoreChanged;

        public int Score { get; private set; }

        public ScoreViewModel(ScoreCounter scoreCounter)
        {
            _scoreCounter = scoreCounter;
        }

        public void Initialize()
        {
            _scoreCounter.ScoreChanged += OnScoreChanged;

            OnScoreChanged(_scoreCounter.Score);
        }

        public void Dispose()
        {
            _scoreCounter.ScoreChanged -= OnScoreChanged;
        }

        private void OnScoreChanged(int score)
        {
            Score = score;
            ScoreChanged?.Invoke(Score);
        }
    }
}
