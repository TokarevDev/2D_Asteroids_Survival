using System;
using System.Collections.Generic;
using Game.Core.Enemies;
using Game.Core.Physics;
using Game.Core.Projectiles;
using Zenject;

namespace Game.Gameplay.Projectiles
{
    public sealed class ProjectileEnemyCollisionController : IFixedTickable
    {
        public event Action<ProjectileEntity, EnemyEntity> CollisionDetected;

        private readonly ProjectileRegistry _projectileRegistry;
        private readonly EnemyRegistry _enemyRegistry;
        private readonly CircleCollisionDetector2D _collisionDetector;

        public ProjectileEnemyCollisionController(ProjectileRegistry projectileRegistry, EnemyRegistry enemyRegistry,
            CircleCollisionDetector2D collisionDetector)
        {
            _projectileRegistry = projectileRegistry ?? throw new ArgumentNullException(nameof(projectileRegistry));

            _enemyRegistry = enemyRegistry ?? throw new ArgumentNullException(nameof(enemyRegistry));

            _collisionDetector = collisionDetector ?? throw new ArgumentNullException(nameof(collisionDetector));
        }

        public void FixedTick()
        {
            IReadOnlyList<ProjectileEntity> projectiles = _projectileRegistry.Projectiles;
            IReadOnlyList<EnemyEntity> enemies = _enemyRegistry.Enemies;

            for (int projectileIndex = projectiles.Count - 1; projectileIndex >= 0; projectileIndex--)
            {
                ProjectileEntity projectile = projectiles[projectileIndex];

                for (int enemyIndex = enemies.Count - 1; enemyIndex >= 0; enemyIndex--)
                {
                    EnemyEntity enemy = enemies[enemyIndex];

                    if (!_collisionDetector.Intersects(projectile.PhysicsBody, enemy.PhysicsBody))
                    {
                        continue;
                    }

                    CollisionDetected?.Invoke(projectile, enemy);
                    break;
                }
            }
        }
    }
}
