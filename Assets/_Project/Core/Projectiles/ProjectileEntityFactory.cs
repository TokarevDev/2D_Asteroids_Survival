using System;
using Game.Core.Configuration;
using UnityEngine;

namespace Game.Core.Projectiles
{
    public sealed class ProjectileEntityFactory
    {
        private readonly IGameConfigProvider _configProvider;

        public ProjectileEntityFactory(IGameConfigProvider configProvider)
        {
            _configProvider = configProvider ?? throw new ArgumentNullException(nameof(configProvider));
        }

        public ProjectileEntity Create()
        {
            PlayerConfig config = _configProvider.Player;

            return new ProjectileEntity(Vector2.zero, Vector2.zero, 0f,
                config.BulletCollisionRadius, config.BulletMass,
                config.BulletDamage, config.BulletLifetimeSeconds);
        }
    }
}
