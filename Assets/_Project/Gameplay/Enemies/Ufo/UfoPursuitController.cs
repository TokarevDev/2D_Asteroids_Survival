using System;
using System.Collections.Generic;
using Game.Core.Configuration;
using Game.Core.Enemies;
using Game.Core.World;
using Game.Gameplay.Player;
using UnityEngine;
using Zenject;

namespace Game.Gameplay.Enemies.Ufo
{
    public sealed class UfoPursuitController : IFixedTickable
    {
        private readonly EnemyRegistry _enemyRegistry;
        private readonly PlayerPhysicsController _playerController;
        private readonly UfoPursuitMovement _movement;

        public UfoPursuitController(EnemyRegistry enemyRegistry, PlayerPhysicsController playerController,
            ToroidalWorld2D world, IGameConfigProvider configProvider)
        {
            _enemyRegistry = enemyRegistry ?? throw new ArgumentNullException(nameof(enemyRegistry));

            _playerController = playerController ?? throw new ArgumentNullException(nameof(playerController));

            if (world == null)
            {
                throw new ArgumentNullException(nameof(world));
            }

            if (configProvider == null)
            {
                throw new ArgumentNullException(nameof(configProvider));
            }

            float speed = configProvider.Enemy.GetParameters(EnemyType.Ufo).Speed;

            _movement = new UfoPursuitMovement(world, speed);
        }

        public void FixedTick()
        {
            IReadOnlyList<EnemyEntity> enemies = _enemyRegistry.Enemies;
            Vector2 playerPosition = _playerController.Body.Position;

            for (int i = 0; i < enemies.Count; i++)
            {
                EnemyEntity enemy = enemies[i];

                if (enemy.Type != EnemyType.Ufo || !enemy.HasEnteredWorld)
                {
                    continue;
                }

                _movement.Step(enemy.PhysicsBody, playerPosition);
            }
        }
    }
}
