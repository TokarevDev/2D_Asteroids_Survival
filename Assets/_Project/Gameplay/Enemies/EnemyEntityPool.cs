using System;
using Game.Core.Configuration;
using Game.Core.Enemies;
using Game.Gameplay.Pooling;
using UnityEngine;

namespace Game.Gameplay.Enemies
{
    public sealed class EnemyEntityPool
    {
        private readonly EnemyEntityFactory _factory;
        private readonly IGameConfigProvider _configProvider;
        private readonly ObjectPool<EnemyEntity> _pool;

        public EnemyEntityPool(EnemyEntityFactory factory, IGameConfigProvider configProvider)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _configProvider = configProvider ?? throw new ArgumentNullException(nameof(configProvider));

            WorldConfig worldConfig = _configProvider.World;

            int initialCapacity = worldConfig.InitialAsteroidPoolSize + worldConfig.InitialUfoPoolSize;

            _pool = new ObjectPool<EnemyEntity>(CreateEntity, initialCapacity);
        }

        public EnemyEntity Get(EnemyType type, Vector2 position, Vector2 velocity, float rotationDegrees)
        {
            EnemyEntity enemy = _pool.Get();

            EnemyParameters parameters = _configProvider.Enemy.GetParameters(type);

            enemy.Reset(type, position, velocity, rotationDegrees, parameters.CollisionRadius, parameters.Mass);
            return enemy;
        }

        public bool Return(EnemyEntity enemy)
        {
            return _pool.Return(enemy);
        }

        private EnemyEntity CreateEntity()
        {
            return _factory.Create(EnemyType.LargeAsteroid, Vector2.zero, Vector2.zero, 0f);
        }
    }
}
