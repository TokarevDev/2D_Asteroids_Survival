using System;
using Game.Core.Configuration;
using UnityEngine;

namespace Game.Core.Enemies
{
    public sealed class EnemyEntityFactory
    {
        private readonly IGameConfigProvider _configProvider;

        public EnemyEntityFactory(IGameConfigProvider configProvider)
        {
            _configProvider = configProvider ?? throw new ArgumentNullException(nameof(configProvider));
        }

        public EnemyEntity Create(EnemyType type, Vector2 position, Vector2 velocity, float rotationDegrees)
        {
            EnemyParameters parameters = _configProvider.Enemy.GetParameters(type);

            return new EnemyEntity(type, position, velocity, rotationDegrees, parameters.CollisionRadius,
                parameters.Mass);
        }
    }
}
