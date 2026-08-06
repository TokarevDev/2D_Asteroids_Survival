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
        private readonly ProjectileLifecycleService _lifecycleService;

        public ProjectileLifetimeController(ProjectileRegistry registry, ProjectileLifecycleService lifecycleService)
        {
            _registry = registry ?? throw new ArgumentNullException(nameof(registry));

            _lifecycleService = lifecycleService ?? throw new ArgumentNullException(nameof(lifecycleService));
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
                    _lifecycleService.Despawn(projectile);
                }
            }
        }
    }
}
