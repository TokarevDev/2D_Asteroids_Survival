using System;
using Game.Core.Configuration;
using Game.Core.World;
using UnityEngine;
using Zenject;

namespace Game.Gameplay.World
{
    public sealed class CameraWorldBoundsSynchronizer : IInitializable, ITickable
    {
        private readonly Camera _camera;
        private readonly ToroidalWorld2D _world;
        private readonly float _baseWidth;
        private readonly float _baseHeight;

        private float _lastAspect = -1f;

        public CameraWorldBoundsSynchronizer(CameraProvider cameraProvider, IGameConfigProvider configProvider,
            ToroidalWorld2D world)
        {
            if (cameraProvider == null)
            {
                throw new ArgumentNullException(nameof(cameraProvider));
            }

            if (configProvider == null)
            {
                throw new ArgumentNullException(nameof(configProvider));
            }

            _camera = cameraProvider.Camera;
            _world = world ?? throw new ArgumentNullException(nameof(world));

            WorldConfig config = configProvider.World;
            _baseWidth = config.Width;
            _baseHeight = config.Height;
        }

        public void Initialize()
        {
            if (!_camera.orthographic)
            {
                throw new InvalidOperationException("CameraWorldBoundsSynchronizer requires an orthographic camera");
            }

            Synchronize();
        }

        public void Tick()
        {
            if (Mathf.Approximately(_camera.aspect, _lastAspect))
            {
                return;
            }

            Synchronize();
        }

        private void Synchronize()
        {
            float aspect = _camera.aspect;

            if (aspect <= 0f)
            {
                return;
            }

            float baseAspect = _baseWidth / _baseHeight;
            float width;
            float height;

            if (aspect >= baseAspect)
            {
                height = _baseHeight;
                width = height * aspect;
            }
            else
            {
                width = _baseWidth;
                height = width / aspect;
            }

            _camera.orthographicSize = height * 0.5f;
            _world.SetSize(width, height);
            _lastAspect = aspect;
        }
    }
}
