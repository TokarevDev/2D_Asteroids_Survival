using System;
using System.Collections.Generic;
using Game.Core.Configuration;
using Game.Core.Projectiles;
using Game.Gameplay.Pooling;
using UnityEngine;
using Zenject;

namespace Game.Gameplay.Projectiles
{
    public sealed class ProjectilePool : MonoBehaviour
    {
        [SerializeField] private Projectile _projectilePrefab;

        private readonly Dictionary<ProjectileEntity, Projectile> _projectilesByEntity = new();

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

            _projectilesByEntity.Clear();
            _pool.Clear();
        }

        public bool TrySpawn(Vector2 position, Vector2 direction, float rotationDegrees)
        {
            Projectile projectile = _pool.Get();
            ProjectileEntity entity;
            bool isSpawned;

            try
            {
                isSpawned = _lifecycleService.TrySpawn(projectile.PhysicsView, position, direction, rotationDegrees,
                    out entity);
            }
            catch
            {
                RollbackSpawn(projectile);
                throw;
            }

            if (!isSpawned)
            {
                ReturnRentedProjectile(projectile);
                return false;
            }

            try
            {
                if (!_projectilesByEntity.TryAdd(entity, projectile))
                {
                    throw new InvalidOperationException("Projectile entity is already associated with a visual");
                }

                projectile.gameObject.SetActive(true);

                return true;
            }
            catch
            {
                RollbackSpawn(projectile);
                throw;
            }
        }

        private bool ReturnProjectile(Projectile projectile)
        {
            if (projectile == null)
            {
                Debug.LogError("Cannot return a null projectile", this);
                return false;
            }

            if (projectile.PhysicsView.IsBound)
            {
                ProjectileEntity entity = projectile.PhysicsView.Entity;

                if (!_projectilesByEntity.TryGetValue(entity, out Projectile registeredProjectile) ||
                    !ReferenceEquals(registeredProjectile, projectile))
                {
                    Debug.LogError("Bound projectile has no matching entity association", projectile);
                    return false;
                }

                if (!_lifecycleService.Despawn(entity))
                {
                    Debug.LogError("Bound projectile physics view was not active", projectile);
                    return false;
                }

                if (!_projectilesByEntity.Remove(entity))
                {
                    throw new InvalidOperationException("Projectile entity association was not registered");
                }
            }

            if (!TryReturnToPool(projectile))
            {
                Debug.LogWarning("Projectile is already in the pool", projectile);
                return false;
            }

            return true;
        }

        public bool Return(ProjectileEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (!_projectilesByEntity.TryGetValue(entity, out Projectile projectile))
            {
                return false;
            }

            return ReturnProjectile(projectile);
        }

        private void RollbackSpawn(Projectile projectile)
        {
            if (projectile.PhysicsView.IsBound)
            {
                ProjectileEntity entity = projectile.PhysicsView.Entity;

                if (!_lifecycleService.Despawn(entity))
                {
                    throw new InvalidOperationException("Failed to despawn projectile during spawn rollback");
                }

                if (_projectilesByEntity.TryGetValue(entity, out Projectile registeredProjectile) &&
                    ReferenceEquals(registeredProjectile, projectile) &&
                    !_projectilesByEntity.Remove(entity))
                {
                    throw new InvalidOperationException(
                        "Failed to remove projectile association during spawn rollback");
                }
            }

            if (!TryReturnToPool(projectile))
            {
                throw new InvalidOperationException("Failed to return projectile during spawn rollback");
            }
        }

        private void ReturnRentedProjectile(Projectile projectile)
        {
            if (!TryReturnToPool(projectile))
            {
                throw new InvalidOperationException("Failed to return projectile after rejected spawn");
            }
        }

        private bool TryReturnToPool(Projectile projectile)
        {
            if (!_pool.Return(projectile))
            {
                return false;
            }

            projectile.gameObject.SetActive(false);
            return true;
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
