using System;
using Game.Core.Enemies;
using Game.Core.Physics;
using UnityEngine;
using Zenject;

namespace Game.Gameplay.Player
{
    public sealed class PlayerEnemyBounceService : IInitializable, IDisposable
    {
        private readonly PlayerEnemyCollisionController _collisionController;
        private readonly PlayerPhysicsController _playerController;
        private readonly ElasticCollisionResolver2D _collisionResolver;

        public PlayerEnemyBounceService(PlayerEnemyCollisionController collisionController,
            PlayerPhysicsController playerController, ElasticCollisionResolver2D collisionResolver)
        {
            _collisionController = collisionController ?? throw new ArgumentNullException(nameof(collisionController));

            _playerController = playerController ?? throw new ArgumentNullException(nameof(playerController));

            _collisionResolver = collisionResolver ?? throw new ArgumentNullException(nameof(collisionResolver));
        }

        public void Initialize()
        {
            _collisionController.DetailedCollisionDetected += OnCollisionDetected;
        }

        public void Dispose()
        {
            _collisionController.DetailedCollisionDetected -= OnCollisionDetected;
        }

        private void OnCollisionDetected(EnemyEntity enemy, Vector2 displacement)
        {
            _collisionResolver.Resolve(_playerController.Body, enemy.PhysicsBody, displacement);
        }
    }
}
