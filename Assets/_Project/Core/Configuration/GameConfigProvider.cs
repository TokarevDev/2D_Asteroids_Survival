using System;

namespace Game.Core.Configuration
{
    public sealed class GameConfigProvider : IGameConfigProvider
    {
        private PlayerConfig _player;
        private EnemyConfig _enemy;
        private WorldConfig _world;

        public bool IsInitialized { get; private set; }

        public PlayerConfig Player =>
            _player ?? throw new InvalidOperationException(
                "Player configuration has not been initialized");

        public EnemyConfig Enemy =>
            _enemy ?? throw new InvalidOperationException(
                "Enemy configuration has not been initialized");

        public WorldConfig World =>
            _world ?? throw new InvalidOperationException(
                "World configuration has not been initialized");

        public void Initialize(PlayerConfig player, EnemyConfig enemy, WorldConfig world)
        {
            if (IsInitialized)
            {
                throw new InvalidOperationException("Game configuration has already been initialized");
            }

            _player = player ?? throw new ArgumentNullException(nameof(player));
            _enemy = enemy ?? throw new ArgumentNullException(nameof(enemy));
            _world = world ?? throw new ArgumentNullException(nameof(world));

            IsInitialized = true;
        }
    }
}
