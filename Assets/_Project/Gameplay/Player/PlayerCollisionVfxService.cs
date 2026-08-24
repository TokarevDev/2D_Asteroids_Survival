using System;
using Zenject;

namespace Game.Gameplay.Player
{
    public sealed class PlayerCollisionVfxService : IInitializable, IDisposable
    {
        private readonly PlayerEnemyCollisionController _collisionController;
        private readonly PlayerPhysicsController _playerController;
        private readonly PlayerCollisionVfxPool _vfxPool;

        public PlayerCollisionVfxService(PlayerEnemyCollisionController collisionController,
            PlayerPhysicsController playerController, PlayerCollisionVfxPool vfxPool)
        {
            _collisionController = collisionController ?? throw new ArgumentNullException(nameof(collisionController));

            _playerController = playerController ?? throw new ArgumentNullException(nameof(playerController));

            _vfxPool = vfxPool ?? throw new ArgumentNullException(nameof(vfxPool));
        }

        public void Initialize()
        {
            _collisionController.CollisionDetected += OnCollisionDetected;
        }

        public void Dispose()
        {
            _collisionController.CollisionDetected -= OnCollisionDetected;
        }

        private void OnCollisionDetected()
        {
            _vfxPool.Play(_playerController.Body.Position);
        }
    }
}
