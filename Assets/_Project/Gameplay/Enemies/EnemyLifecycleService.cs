using System;
using Game.Core.Enemies;
using Game.Core.Physics;
using UnityEngine;

namespace Game.Gameplay.Enemies
{
    public sealed class EnemyLifecycleService
    {
        private readonly EnemyEntityPool _pool;
        private readonly EnemyRegistry _registry;
        private readonly CustomPhysicsWorld2D _physicsWorld;
        private readonly EnemyPhysicsViewSynchronizer _viewSynchronizer;

        public EnemyLifecycleService(EnemyEntityPool pool, EnemyRegistry registry, CustomPhysicsWorld2D physicsWorld,
            EnemyPhysicsViewSynchronizer viewSynchronizer)
        {
            _pool = pool ?? throw new ArgumentNullException(nameof(pool));
            _registry = registry ?? throw new ArgumentNullException(nameof(registry));
            _physicsWorld = physicsWorld ?? throw new ArgumentNullException(nameof(physicsWorld));
            _viewSynchronizer = viewSynchronizer ?? throw new ArgumentNullException(nameof(viewSynchronizer));
        }

        public EnemyEntity Spawn(EnemyPhysicsView view, EnemyType type, Vector2 position, Vector2 velocity,
            float rotationDegrees)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            if (view.IsBound)
            {
                throw new InvalidOperationException("Enemy physics view is already bound");
            }

            EnemyEntity enemy = _pool.Get(type, position, velocity, rotationDegrees);

            if (!_registry.Register(enemy))
            {
                throw new InvalidOperationException("Enemy entity is already registered");
            }

            if (!_physicsWorld.Register(enemy.PhysicsBody))
            {
                _registry.Unregister(enemy);
                ReturnToPool(enemy);

                throw new InvalidOperationException("Enemy physics body is already registered");
            }

            try
            {
                view.Bind(enemy);

                if (!_viewSynchronizer.Register(view))
                {
                    throw new InvalidOperationException("Enemy physics view is already registered");
                }

                return enemy;
            }
            catch
            {
                if (view.IsBound && ReferenceEquals(view.Entity, enemy))
                {
                    view.Unbind();
                }

                _physicsWorld.Unregister(enemy.PhysicsBody);
                _registry.Unregister(enemy);
                ReturnToPool(enemy);

                throw;
            }
        }

        public bool Despawn(EnemyPhysicsView view)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            if (!view.IsBound)
            {
                return false;
            }

            EnemyEntity enemy = view.Entity;

            if (!_viewSynchronizer.Unregister(view))
            {
                throw new InvalidOperationException("Enemy physics view is not registered");
            }

            view.Unbind();

            bool removeFromRegistry = _registry.Unregister(enemy);
            bool removeFromPhysics = _physicsWorld.Unregister(enemy.PhysicsBody);

            if (!removeFromRegistry && !removeFromPhysics)
            {
                return false;
            }

            ReturnToPool(enemy);

            if (removeFromRegistry != removeFromPhysics)
            {
                throw new InvalidOperationException("Enemy registry and physics world were out of sync");
            }

            return true;
        }

        private void ReturnToPool(EnemyEntity enemy)
        {
            if (!_pool.Return(enemy))
            {
                throw new InvalidOperationException("Enemy entity is already in the pool");
            }
        }
    }
}
