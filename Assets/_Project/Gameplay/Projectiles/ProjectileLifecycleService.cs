using System;
using Game.Core.Configuration;
using Game.Core.Physics;
using Game.Core.Projectiles;
using UnityEngine;

namespace Game.Gameplay.Projectiles
{
    public sealed class ProjectileLifecycleService
    {
        private readonly ProjectileEntityPool _pool;
        private readonly ProjectileRegistry _registry;
        private readonly CustomPhysicsWorld2D _physicsWorld;
        private readonly ProjectilePhysicsViewSynchronizer _viewSynchronizer;

        private readonly int _maxActiveProjectiles;

        public ProjectileLifecycleService(ProjectileEntityPool pool, ProjectileRegistry registry,
            CustomPhysicsWorld2D physicsWorld, ProjectilePhysicsViewSynchronizer viewSynchronizer,
            IGameConfigProvider configProvider)
        {
            _pool = pool ?? throw new ArgumentNullException(nameof(pool));

            _registry = registry ?? throw new ArgumentNullException(nameof(registry));

            _physicsWorld = physicsWorld ?? throw new ArgumentNullException(nameof(physicsWorld));

            _viewSynchronizer = viewSynchronizer ?? throw new ArgumentNullException(nameof(viewSynchronizer));

            if (configProvider == null)
            {
                throw new ArgumentNullException(nameof(configProvider));
            }

            _maxActiveProjectiles = configProvider.Player.MaxActiveBullets;
        }

        public bool TrySpawn(ProjectilePhysicsView view, Vector2 position, Vector2 direction, float rotationDegrees,
            out ProjectileEntity projectile)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            if (view.IsBound)
            {
                throw new InvalidOperationException("Projectile physics view is already bound");
            }

            projectile = null;

            if (_registry.Count >= _maxActiveProjectiles)
            {
                return false;
            }

            ProjectileEntity candidate = _pool.Get(position, direction, rotationDegrees);

            if (!_registry.Register(candidate))
            {
                ReturnToPool(candidate);

                throw new InvalidOperationException("Projectile entity is already registered");
            }

            if (!_physicsWorld.Register(candidate.PhysicsBody))
            {
                _registry.Unregister(candidate);
                ReturnToPool(candidate);

                throw new InvalidOperationException("Projectile physics body is already registered");
            }

            try
            {
                view.Bind(candidate);

                if (!_viewSynchronizer.Register(view))
                {
                    throw new InvalidOperationException("Projectile physics view is already registered");
                }
            }
            catch
            {
                if (view.IsBound && ReferenceEquals(view.Entity, candidate))
                {
                    view.Unbind();
                }

                _physicsWorld.Unregister(candidate.PhysicsBody);
                _registry.Unregister(candidate);
                ReturnToPool(candidate);
                throw;
            }

            projectile = candidate;
            return true;
        }

        public bool Despawn(ProjectileEntity projectile)
        {
            if (projectile == null)
            {
                throw new ArgumentNullException(nameof(projectile));
            }

            if (!_viewSynchronizer.TryGetView(projectile, out ProjectilePhysicsView view))
            {
                return false;
            }

            if (!_viewSynchronizer.Unregister(view))
            {
                throw new InvalidOperationException("Projectile physics view is not registered");
            }

            view.Unbind();

            bool removedFromRegistry = _registry.Unregister(projectile);

            bool removedFromPhysics = _physicsWorld.Unregister(projectile.PhysicsBody);

            if (!removedFromRegistry && !removedFromPhysics)
            {
                return false;
            }

            ReturnToPool(projectile);

            if (removedFromRegistry != removedFromPhysics)
            {
                throw new InvalidOperationException("Projectile registry and physics world were out of sync");
            }

            return true;
        }

        private void ReturnToPool(ProjectileEntity projectile)
        {
            if (!_pool.Return(projectile))
            {
                throw new InvalidOperationException("Projectile entity is already in the pool");
            }
        }
    }
}
