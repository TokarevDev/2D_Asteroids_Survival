using System;
using System.Collections.Generic;
using Game.Core.Enemies;
using Game.Core.Physics;
using Game.Core.World;
using UnityEngine;
using Zenject;

namespace Game.Gameplay.Player
{
    public sealed class PlayerEnemyCollisionController : IFixedTickable
    {
        public event Action<EnemyEntity> CollisionDetected;

        private readonly PlayerPhysicsController _playerController;
        private readonly EnemyRegistry _enemyRegistry;
        private readonly CircleCollisionDetector2D _collisionDetector;
        private readonly ToroidalWorld2D _world;

        public PlayerEnemyCollisionController(PlayerPhysicsController playerController, EnemyRegistry enemyRegistry,
            CircleCollisionDetector2D collisionDetector, ToroidalWorld2D world)
        {
            _playerController = playerController ?? throw new ArgumentNullException(nameof(playerController));

            _enemyRegistry = enemyRegistry ?? throw new ArgumentNullException(nameof(enemyRegistry));

            _collisionDetector = collisionDetector ?? throw new ArgumentNullException(nameof(collisionDetector));

            _world = world ?? throw new ArgumentNullException(nameof(world));
        }

        public void FixedTick()
        {
            CustomPhysicsBody2D playerBody = _playerController.Body;
            IReadOnlyList<EnemyEntity> enemies = _enemyRegistry.Enemies;

            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                EnemyEntity enemy = enemies[i];
                if (!enemy.HasEnteredWorld)
                {
                    continue;
                }

                CustomPhysicsBody2D enemyBody = enemy.PhysicsBody;

                Vector2 displacement = _world.GetShortestDisplacement(playerBody.Position, enemyBody.Position);

                if (!_collisionDetector.Intersects(playerBody, enemyBody, displacement))
                {
                    continue;
                }

                CollisionDetected?.Invoke(enemy);
            }
        }
    }
}
