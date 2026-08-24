using System;
using Game.Gameplay.Session;
using Zenject;

namespace Game.UI.HUD
{
    public sealed class TimerViewModel : IInitializable, IDisposable
    {
        private const int SecondsPerMinute = 60;

        public event Action<int, int> TimeChanged;

        private readonly SurvivalTimer _survivalTimer;

        public int Minutes { get; private set; }
        public int Seconds { get; private set; }

        public TimerViewModel(SurvivalTimer survivalTimer)
        {
            _survivalTimer = survivalTimer ?? throw new ArgumentNullException(nameof(survivalTimer));
        }

        public void Initialize()
        {
            _survivalTimer.ElapsedSecondsChanged += OnElapsedSecondsChanged;

            OnElapsedSecondsChanged(_survivalTimer.ElapsedSeconds);
        }

        public void Dispose()
        {
            _survivalTimer.ElapsedSecondsChanged -= OnElapsedSecondsChanged;
        }

        private void OnElapsedSecondsChanged(int elapsedSeconds)
        {
            Minutes = elapsedSeconds / SecondsPerMinute;
            Seconds = elapsedSeconds % SecondsPerMinute;

            TimeChanged?.Invoke(Minutes, Seconds);
        }
    }
}
