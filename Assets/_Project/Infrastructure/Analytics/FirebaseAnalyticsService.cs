using System;
using Firebase.Analytics;
using Game.Core.Analytics;
using UnityEngine;

namespace Game.Infrastructure.Analytics
{
    public sealed class FirebaseAnalyticsService : IAnalyticsService
    {
        private const string GameStartedEvent = "game_started";
        private const string GameEndedEvent = "game_ended";
        private const string ScoreParameter = "score";
        private const string DurationSecondsParameter = "duration_seconds";

        private readonly FirebaseInitializer _initializer;

        public FirebaseAnalyticsService(FirebaseInitializer initializer)
        {
            _initializer = initializer ?? throw new ArgumentNullException(nameof(initializer));
        }

        public void LogGameStarted()
        {
            if (!_initializer.IsInitialized)
            {
                Debug.LogWarning("Game started event was skipped because Firebase is not initialized");
                return;
            }

            FirebaseAnalytics.LogEvent(GameStartedEvent);
        }

        public void LogGameEnded(int score, float durationSeconds)
        {
            if (!_initializer.IsInitialized)
            {
                Debug.LogWarning("Game ended event was skipped because Firebase is not initialized");
                return;
            }

            FirebaseAnalytics.LogEvent(GameEndedEvent, new Parameter(ScoreParameter, score),
                new Parameter(DurationSecondsParameter, durationSeconds));
        }
    }
}
