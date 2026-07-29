using System;
using UnityEngine;
using Zenject;

namespace Game.Gameplay
{
    public sealed class SurvivalTimer : ITickable
    {
        public event Action<int> ElapsedSecondsChanged;

        private float _elapsedTime;

        public int ElapsedSeconds { get; private set; }

        public void Tick()
        {
            _elapsedTime += Time.deltaTime;

            int elapsedSeconds = (int)_elapsedTime;

            if (elapsedSeconds == ElapsedSeconds)
            {
                return;
            }

            ElapsedSeconds = elapsedSeconds;
            ElapsedSecondsChanged?.Invoke(ElapsedSeconds);
        }
    }
}
