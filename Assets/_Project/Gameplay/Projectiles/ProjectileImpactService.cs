using System;
using Game.Core.Enemies;
using Game.Core.Projectiles;
using UnityEngine;
using Zenject;

namespace Game.Gameplay.Projectiles
{
    public sealed class ProjectileImpactService : IInitializable, IDisposable
    {
        private readonly ProjectileEnemyCollisionController _collisionController;
        private readonly ProjectilePool _projectilePool;
        private readonly AsteroidPool _asteroidPool;
        private readonly AsteroidFragmentSpawner _fragmentSpawner;

        public ProjectileImpactService(ProjectileEnemyCollisionController collisionController,
            ProjectilePool projectilePool, AsteroidPool asteroidPool, AsteroidFragmentSpawner fragmentSpawner)
        {
            _collisionController = collisionController ?? throw new ArgumentNullException(nameof(collisionController));

            _projectilePool = projectilePool ?? throw new ArgumentNullException(nameof(projectilePool));

            _asteroidPool = asteroidPool ?? throw new ArgumentNullException(nameof(asteroidPool));

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
            if (!_asteroidPool.TryGetByEntity(enemy, out Asteroid asteroid))
            {
                throw new InvalidOperationException("Detected enemy entity has no associated asteroid");
            }

            int damage = projectile.Damage;

            bool shouldSpawnFragments = enemy.Type == EnemyType.LargeAsteroid && damage >= asteroid.CurrentHealth;

            Vector2 parentPosition = enemy.PhysicsBody.Position;
            Vector2 parentVelocity = enemy.PhysicsBody.Velocity;

            if (!_projectilePool.Return(projectile))
            {
                throw new InvalidOperationException("Detected projectile entity has no associated visual");
            }

            asteroid.TakeDamage(damage);

            if (shouldSpawnFragments)
            {
                _fragmentSpawner.Spawn(parentPosition, parentVelocity);
            }
        }
    }
}
