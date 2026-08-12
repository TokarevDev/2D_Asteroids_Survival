using System;
using Game.Core.Configuration;
using UnityEngine;
using Zenject;

namespace Game.Gameplay.Player
{
    public sealed class PlayerCollisionVfxPool : MonoBehaviour
    {
        [SerializeField] private PlayerCollisionVfx _prefab;

        private IGameConfigProvider _configProvider;
        private ObjectPool<PlayerCollisionVfx> _pool;

        [Inject]
        private void Construct(IGameConfigProvider configProvider)
        {
            _configProvider = configProvider ?? throw new ArgumentNullException(nameof(configProvider));
        }

        private void Awake()
        {
            if (_prefab == null)
            {
                Debug.LogError("Player collision VFX prefab reference is missing", this);
                enabled = false;
                return;
            }

            _pool = new ObjectPool<PlayerCollisionVfx>(CreateVfx, _configProvider.World.InitialCollisionVfxPoolSize);
        }

        private void OnDestroy()
        {
            if (_pool == null)
            {
                return;
            }

            for (int i = 0; i < _pool.CreatedItems.Count; i++)
            {
                PlayerCollisionVfx vfx = _pool.CreatedItems[i];

                if (vfx != null)
                {
                    vfx.Finished -= OnVfxFinished;
                }
            }

            _pool.Clear();
        }

        public void Play(Vector2 position)
        {
            PlayerCollisionVfx vfx = _pool.Get();
            vfx.Play(position);
        }

        private PlayerCollisionVfx CreateVfx()
        {
            PlayerCollisionVfx vfx = Instantiate(_prefab, transform);

            vfx.Finished += OnVfxFinished;
            vfx.gameObject.SetActive(false);

            return vfx;
        }

        private void OnVfxFinished(PlayerCollisionVfx vfx)
        {
            if (!_pool.Return(vfx))
            {
                Debug.LogWarning("Player collision VFX is already in the pool", vfx);
                return;
            }

            vfx.gameObject.SetActive(false);
        }
    }
}
