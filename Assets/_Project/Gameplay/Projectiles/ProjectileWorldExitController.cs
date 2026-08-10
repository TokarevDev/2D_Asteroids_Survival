using System;
using System.Collections.Generic;
using Game.Core.Projectiles;
using Game.Core.World;
using Zenject;

namespace Game.Gameplay.Projectiles
{
    public sealed class ProjectileWorldExitController : IFixedTickable
    {
        private readonly ProjectileRegistry _registry;
        private readonly ProjectilePool _projectilePool;
        private readonly ToroidalWorld2D _world;

        public ProjectileWorldExitController(ProjectileRegistry registry, ProjectilePool projectilePool,
            ToroidalWorld2D world)
        {
            _registry = registry ?? throw new ArgumentNullException(nameof(registry));

            _projectilePool = projectilePool ?? throw new ArgumentNullException(nameof(projectilePool));

            _world = world ?? throw new ArgumentNullException(nameof(world));
        }

        public void FixedTick()
        {
            IReadOnlyList<ProjectileEntity> projectiles = _registry.Projectiles;

            for (int i = projectiles.Count - 1; i >= 0; i--)
            {
                ProjectileEntity projectile = projectiles[i];

                if (_world.Contains(projectile.PhysicsBody.Position))
                {
                    continue;
                }

                if (!_projectilePool.Return(projectile))
                {
                    throw new InvalidOperationException("Projectile outside the world has no associated visual");
                }
            }
        }
    }
}
