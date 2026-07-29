using System;
using UnityEngine;
using Zenject;

namespace Game.Gameplay
{
    public sealed class GamePauseService : IInitializable, IDisposable
    {
        public void Dispose()
        {
            Resume();
        }

        public void Initialize()
        {
            Resume();
        }

        public void Pause()
        {
            Time.timeScale = 0f;
        }

        private void Resume()
        {
            Time.timeScale = 1f;
        }
    }
}
