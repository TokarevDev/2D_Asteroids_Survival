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

        public EnemyLifecycleService(EnemyEntityPool pool, EnemyRegistry registry, CustomPhysicsWorld2D physicsWorld)
        {
            _pool = pool ?? throw new ArgumentNullException(nameof(pool));
            _registry = registry ?? throw new ArgumentNullException(nameof(registry));
            _physicsWorld = physicsWorld ?? throw new ArgumentNullException(nameof(physicsWorld));
        }

        public EnemyEntity Spawn(EnemyType type, Vector2 position, Vector2 velocity, float rotationDegrees)
        {
            EnemyEntity enemy = _pool.Get(type, position, velocity, rotationDegrees);

            if (!_registry.Register(enemy))
            {
                throw new InvalidOperationException("Enemy entity is already registered");
            }

            if (_physicsWorld.Register(enemy.PhysicsBody))
            {
                return enemy;
            }

            _registry.Unregister(enemy);
            ReturnToPool(enemy);

            throw new InvalidOperationException("Enemy physics body is already registered");
        }

        public bool Despawn(EnemyEntity enemy)
        {
            if (enemy == null)
            {
                throw new ArgumentNullException(nameof(enemy));
            }

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
