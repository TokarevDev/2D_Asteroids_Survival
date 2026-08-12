using System;
using Game.Core.Enemies;
using Game.Core.Player;
using UnityEngine;
using Zenject;

namespace Game.Gameplay.Player
{
    public sealed class PlayerCollisionDamageService : IInitializable, IDisposable
    {
        private const int CollisionDamage = 1;

        private readonly PlayerEnemyCollisionController _collisionController;
        private readonly PlayerInvulnerability _invulnerability;
        private readonly PlayerHealth _playerHealth;

        public PlayerCollisionDamageService(PlayerEnemyCollisionController collisionController,
            PlayerInvulnerability invulnerability, PlayerHealth playerHealth)
        {
            _collisionController = collisionController ?? throw new ArgumentNullException(nameof(collisionController));

            _invulnerability = invulnerability ?? throw new ArgumentNullException(nameof(invulnerability));

            _playerHealth = playerHealth ?? throw new ArgumentNullException(nameof(playerHealth));
        }

        public void Initialize()
        {
            _collisionController.CollisionDetected += OnCollisionDetected;
        }

        public void Dispose()
        {
            _collisionController.CollisionDetected -= OnCollisionDetected;
        }

        private void OnCollisionDetected(EnemyEntity enemy, Vector2 displacement)
        {
            if (_playerHealth.IsDead)
            {
                return;
            }

            if (!_invulnerability.TryActivate())
            {
                return;
            }

            _playerHealth.TakeDamage(CollisionDamage);
        }
    }
}
