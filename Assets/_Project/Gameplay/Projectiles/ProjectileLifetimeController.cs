using System;
using System.Collections.Generic;
using Game.Core.Projectiles;
using UnityEngine;
using Zenject;

namespace Game.Gameplay.Projectiles
{
    public sealed class ProjectileLifetimeController : IFixedTickable
    {
        private readonly ProjectileRegistry _registry;
        private readonly ProjectilePool _projectilePool;

        public ProjectileLifetimeController(ProjectileRegistry registry, ProjectilePool projectilePool)
        {
            _registry = registry ?? throw new ArgumentNullException(nameof(registry));

            _projectilePool = projectilePool ?? throw new ArgumentNullException(nameof(projectilePool));
        }

        public void FixedTick()
        {
            IReadOnlyList<ProjectileEntity> projectiles = _registry.Projectiles;

            for (int i = projectiles.Count - 1; i >= 0; i--)
            {
                ProjectileEntity projectile = projectiles[i];

                projectile.AdvanceLifetime(Time.fixedDeltaTime);

                if (projectile.IsExpired)
                {
                    if (!_projectilePool.Return(projectile))
                    {
                        throw new InvalidOperationException("Expired projectile has no associated visual");
                    }
                }
            }
        }
    }
}
