using System;
using Game.Core.Enemies;
using Game.Core.Projectiles;
using Zenject;

namespace Game.Gameplay.Projectiles
{
    public sealed class ProjectileImpactService : IInitializable, IDisposable
    {
        private readonly ProjectileEnemyCollisionController _collisionController;
        private readonly ProjectilePool _projectilePool;
        private readonly AsteroidPool _asteroidPool;

        public ProjectileImpactService(ProjectileEnemyCollisionController collisionController,
            ProjectilePool projectilePool, AsteroidPool asteroidPool)
        {
            _collisionController = collisionController ?? throw new ArgumentNullException(nameof(collisionController));

            _projectilePool = projectilePool ?? throw new ArgumentNullException(nameof(projectilePool));

            _asteroidPool = asteroidPool ?? throw new ArgumentNullException(nameof(asteroidPool));
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

            if (!_projectilePool.Return(projectile))
            {
                throw new InvalidOperationException("Detected projectile entity has no associated visual");
            }

            asteroid.TakeDamage(damage);
        }
    }
}
