using System;
using Game.Gameplay;
using Zenject;

namespace Game.UI
{
    public sealed class TimerViewModel : IInitializable, IDisposable
    {
        private readonly SurvivalTimer _survivalTimer;

        public event Action<int, int> TimeChanged;

        public int Minutes { get; private set; }
        public int Seconds { get; private set; }

        public TimerViewModel(SurvivalTimer survivalTimer)
        {
            _survivalTimer = survivalTimer;
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
            Minutes = elapsedSeconds / 60;
            Seconds = elapsedSeconds % 60;

            TimeChanged?.Invoke(Minutes, Seconds);
        }
    }
}
