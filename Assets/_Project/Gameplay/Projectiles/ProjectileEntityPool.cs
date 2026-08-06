using System;
using Game.Core.Configuration;
using Game.Core.Projectiles;
using UnityEngine;

namespace Game.Gameplay.Projectiles
{
    public sealed class ProjectileEntityPool
    {
        private readonly PlayerConfig _config;
        private readonly ObjectPool<ProjectileEntity> _pool;

        public ProjectileEntityPool(ProjectileEntityFactory factory, IGameConfigProvider configProvider)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            if (configProvider == null)
            {
                throw new ArgumentNullException(nameof(configProvider));
            }

            _config = configProvider.Player;

            _pool = new ObjectPool<ProjectileEntity>(factory.Create, _config.MaxActiveBullets);
        }

        public ProjectileEntity Get(Vector2 position, Vector2 direction, float rotationDegrees)
        {
            if (direction.sqrMagnitude <= 0f)
            {
                throw new ArgumentException("Projectile direction cannot be zero", nameof(direction));
            }

            Vector2 velocity = direction.normalized * _config.BulletSpeed;

            ProjectileEntity projectile = _pool.Get();

            projectile.Reset(position, velocity, rotationDegrees, _config.BulletCollisionRadius, _config.BulletMass,
                _config.BulletDamage, _config.BulletLifetimeSeconds);

            return projectile;
        }

        public bool Return(ProjectileEntity projectile)
        {
            return _pool.Return(projectile);
        }
    }
}
