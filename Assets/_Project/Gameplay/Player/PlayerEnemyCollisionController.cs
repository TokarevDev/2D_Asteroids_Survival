using System;
using System.Collections.Generic;
using Game.Core.Enemies;
using Game.Core.Physics;
using Game.Core.Player;
using UnityEngine;
using Zenject;

namespace Game.Gameplay.Player
{
    public sealed class PlayerEnemyCollisionController : IFixedTickable
    {
        public event Action<EnemyEntity, Vector2> CollisionDetected;

        private readonly PlayerPhysicsController _playerController;
        private readonly EnemyRegistry _enemyRegistry;
        private readonly CircleCollisionDetector2D _collisionDetector;
        private readonly PlayerInvulnerability _invulnerability;

        public PlayerEnemyCollisionController(PlayerPhysicsController playerController,
            PlayerInvulnerability invulnerability, EnemyRegistry enemyRegistry,
            CircleCollisionDetector2D collisionDetector)
        {
            _playerController = playerController ?? throw new ArgumentNullException(nameof(playerController));

            _invulnerability = invulnerability ?? throw new ArgumentNullException(nameof(invulnerability));

            _enemyRegistry = enemyRegistry ?? throw new ArgumentNullException(nameof(enemyRegistry));

            _collisionDetector = collisionDetector ?? throw new ArgumentNullException(nameof(collisionDetector));
        }

        public void FixedTick()
        {
            if (_invulnerability.IsActive)
            {
                return;
            }

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

                Vector2 displacement = enemyBody.Position - playerBody.Position;

                if (!_collisionDetector.Intersects(playerBody, enemyBody, displacement))
                {
                    continue;
                }

                CollisionDetected?.Invoke(enemy, displacement);
                return;
            }
        }
    }
}
