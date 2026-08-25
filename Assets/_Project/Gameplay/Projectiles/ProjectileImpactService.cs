using System;
using Game.Core.Enemies;
using Game.Core.Projectiles;
using Game.Gameplay.Asteroids;
using Game.Gameplay.Combat;
using Game.Gameplay.Enemies;
using UnityEngine;
using Zenject;

namespace Game.Gameplay.Projectiles
{
    public sealed class ProjectileImpactService : IInitializable, IDisposable
    {
        private readonly ProjectileEnemyCollisionController _collisionController;
        private readonly ProjectilePool _projectilePool;
        private readonly EnemyDamageableRegistry _damageableRegistry;
        private readonly AsteroidFragmentSpawner _fragmentSpawner;

        public ProjectileImpactService(ProjectileEnemyCollisionController collisionController,
            ProjectilePool projectilePool, EnemyDamageableRegistry damageableRegistry,
            AsteroidFragmentSpawner fragmentSpawner)
        {
            _collisionController = collisionController ?? throw new ArgumentNullException(nameof(collisionController));

            _projectilePool = projectilePool ?? throw new ArgumentNullException(nameof(projectilePool));

            _damageableRegistry = damageableRegistry ?? throw new ArgumentNullException(nameof(damageableRegistry));

            _fragmentSpawner = fragmentSpawner ?? throw new ArgumentNullException(nameof(fragmentSpawner));
        }

        public void Initialize()
        {
            _collisionController.CollisionDetected += OnCollisionDetected;
        }

        public void Dispose()
        {
            _collisionController.CollisionDetected -= OnCollisionDetected;
        }

        private void OnCollisionDetected(ProjectileEntity projectile, EnemyEntity enemy)
        {
            if (!_damageableRegistry.TryGet(enemy, out IDamageable damageable))
            {
                throw new InvalidOperationException("Enemy entity has no associated damageable");
            }

            int damageAmount = projectile.Damage;
            bool shouldSpawnFragments =
                enemy.Type == EnemyType.LargeAsteroid && damageAmount >= damageable.CurrentHealth;

            Vector2 enemyPosition = enemy.PhysicsBody.Position;
            Vector2 enemyVelocity = enemy.PhysicsBody.Velocity;

            ReturnProjectile(projectile);
            damageable.TakeDamage(damageAmount);

            if (shouldSpawnFragments)
            {
                _fragmentSpawner.Spawn(enemyPosition, enemyVelocity);
            }
        }

        private void ReturnProjectile(ProjectileEntity projectile)
        {
            if (!_projectilePool.Return(projectile))
            {
                throw new InvalidOperationException("Detected projectile entity has no associated visual");
            }
        }
    }
}
