namespace Game.Core.Analytics
{
    public interface IAnalyticsService
    {
        void LogGameStarted();

        void LogGameEnded(int score, float durationSeconds);
    }
}
