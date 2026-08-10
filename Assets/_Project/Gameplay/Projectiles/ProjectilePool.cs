using System;
using Game.Core.Configuration;
using Game.Core.Projectiles;
using Game.Gameplay.Projectiles;
using UnityEngine;
using Zenject;

namespace Game.Gameplay
{
    public sealed class ProjectilePool : MonoBehaviour
    {
        [SerializeField] private Projectile _projectilePrefab;

        private IGameConfigProvider _configProvider;
        private ProjectileLifecycleService _lifecycleService;
        private ObjectPool<Projectile> _pool;

        [Inject]
        private void Construct(IGameConfigProvider configProvider, ProjectileLifecycleService lifecycleService)
        {
            _configProvider = configProvider ?? throw new ArgumentNullException(nameof(configProvider));

            _lifecycleService = lifecycleService ?? throw new ArgumentNullException(nameof(lifecycleService));
        }

        private void Awake()
        {
            if (!ValidateSerializedReferences())
            {
                enabled = false;
                return;
            }

            _pool = new ObjectPool<Projectile>(
                CreateProjectile,
                _configProvider.World.InitialProjectilePoolSize);
        }

        private void OnDestroy()
        {
            if (_pool == null)
            {
                return;
            }

            _pool.Clear();
        }

        public bool TrySpawn(Vector2 position, Vector2 direction, float rotationDegrees)
        {
            Projectile projectile = _pool.Get();

            try
            {
                if (!_lifecycleService.TrySpawn(projectile.PhysicsView, position, direction, rotationDegrees, out _))
                {
                    Return(projectile);
                    return false;
                }

                projectile.gameObject.SetActive(true);

                return true;
            }
            catch
            {
                Return(projectile);
                throw;
            }
        }

        public void Return(Projectile projectile)
        {
            if (projectile == null)
            {
                Debug.LogError("Cannot return a null projectile", this);
                return;
            }

            if (projectile.PhysicsView.IsBound && !_lifecycleService.Despawn(projectile.PhysicsView.Entity))
            {
                Debug.LogError("Bound projectile physics view was not active", projectile);
                return;
            }

            if (!_pool.Return(projectile))
            {
                Debug.LogWarning("Projectile is already in the pool", projectile);
                return;
            }

            projectile.gameObject.SetActive(false);
        }

        public bool Return(ProjectileEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (!TryGetByEntity(entity, out Projectile projectile))
            {
                return false;
            }

            Return(projectile);
            return true;
        }

        private bool TryGetByEntity(ProjectileEntity entity, out Projectile projectile)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            for (int i = 0; i < _pool.CreatedItems.Count; i++)
            {
                Projectile candidate = _pool.CreatedItems[i];

                if (candidate == null || !candidate.PhysicsView.IsBound)
                {
                    continue;
                }

                if (ReferenceEquals(candidate.PhysicsView.Entity, entity))
                {
                    projectile = candidate;
                    return true;
                }
            }

            projectile = null;
            return false;
        }

        private Projectile CreateProjectile()
        {
            Projectile projectile = Instantiate(_projectilePrefab, transform);

            projectile.gameObject.SetActive(false);
            return projectile;
        }

        private bool ValidateSerializedReferences()
        {
            if (_projectilePrefab != null)
            {
                return true;
            }

            Debug.LogError("Projectile prefab reference is missing", this);
            return false;
        }
    }
}
