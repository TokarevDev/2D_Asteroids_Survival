using System;
using Game.Core.Configuration;
using UnityEngine;
using Zenject;

namespace Game.Gameplay.Weapons
{
    public sealed class LaserViewController : IInitializable, ITickable, IDisposable
    {
        private readonly LaserShotService _shotService;
        private readonly LaserView _view;
        private readonly float _visualDurationSeconds;

        private float _remainingSeconds;
        private bool _isVisible;

        public LaserViewController(LaserShotService shotService, LaserView view, IGameConfigProvider configProvider)
        {
            _shotService = shotService ?? throw new ArgumentNullException(nameof(shotService));

            _view = view ?? throw new ArgumentNullException(nameof(view));

            if (configProvider == null)
            {
                throw new ArgumentNullException(nameof(configProvider));
            }

            _visualDurationSeconds = configProvider.Player.LaserVisualDurationSeconds;
        }

        public void Initialize()
        {
            _shotService.Fired += OnLaserFired;
        }

        public void Dispose()
        {
            _shotService.Fired -= OnLaserFired;
        }

        public void Tick()
        {
            if (!_isVisible)
            {
                return;
            }

            _remainingSeconds -= Time.deltaTime;

            if (_remainingSeconds > 0f)
            {
                return;
            }

            _view.Hide();
            _isVisible = false;
        }

        private void OnLaserFired(Vector2 start, Vector2 end)
        {
            _view.Show(start, end);
            _remainingSeconds = _visualDurationSeconds;
            _isVisible = true;
        }
    }
}
