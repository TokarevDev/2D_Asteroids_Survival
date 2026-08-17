using System;

namespace Game.Gameplay.Score
{
    public sealed class ScoreCounter
    {
        public event Action<int> ScoreChanged;

        public int Score { get; private set; }

        public void AddScore(int scoreReward)
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
