using System;
using Game.Core.Enemies;
using Game.Core.Projectiles;
using Game.Gameplay.Enemies.Ufo;
using UnityEngine;
using Zenject;

namespace Game.Gameplay.Projectiles
{
    public sealed class ProjectileImpactService : IInitializable, IDisposable
    {
        private readonly ProjectileEnemyCollisionController _collisionController;
        private readonly ProjectilePool _projectilePool;
        private readonly AsteroidPool _asteroidPool;
        private readonly UfoPool _ufoPool;
        private readonly AsteroidFragmentSpawner _fragmentSpawner;

        public ProjectileImpactService(ProjectileEnemyCollisionController collisionController,
            ProjectilePool projectilePool, AsteroidPool asteroidPool, AsteroidFragmentSpawner fragmentSpawner,
            UfoPool ufoPool)
        {
            _collisionController = collisionController ?? throw new ArgumentNullException(nameof(collisionController));

            _projectilePool = projectilePool ?? throw new ArgumentNullException(nameof(projectilePool));

            _asteroidPool = asteroidPool ?? throw new ArgumentNullException(nameof(asteroidPool));

            _fragmentSpawner = fragmentSpawner ?? throw new ArgumentNullException(nameof(fragmentSpawner));

            _ufoPool = ufoPool ?? throw new ArgumentNullException(nameof(ufoPool));
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
            int damage = projectile.Damage;

            if (_asteroidPool.TryGetByEntity(enemy, out Asteroid asteroid))
            {
                HandleAsteroidCollision(projectile, enemy, asteroid, damage);
                return;
            }

            if (_ufoPool.TryGetByEntity(enemy, out Ufo ufo))
            {
                ReturnProjectile(projectile);
                ufo.TakeDamage(damage);
                return;
            }

            throw new InvalidOperationException("Detected enemy entity has no associated visual");
        }

        private void HandleAsteroidCollision(ProjectileEntity projectile, EnemyEntity enemy, Asteroid asteroid,
            int damage)
        {
            bool shouldSpawnFragments = enemy.Type == EnemyType.LargeAsteroid && damage >= asteroid.CurrentHealth;

            Vector2 parentPosition = enemy.PhysicsBody.Position;
            Vector2 parentVelocity = enemy.PhysicsBody.Velocity;

            ReturnProjectile(projectile);
            asteroid.TakeDamage(damage);

            if (shouldSpawnFragments)
            {
                _fragmentSpawner.Spawn(parentPosition, parentVelocity);
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
