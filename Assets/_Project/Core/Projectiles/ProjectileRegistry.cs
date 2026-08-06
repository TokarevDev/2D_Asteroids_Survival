using System;
using System.Collections.Generic;

namespace Game.Core.Projectiles
{
    public sealed class ProjectileRegistry
    {
        private readonly List<ProjectileEntity> _projectiles = new();

        public IReadOnlyList<ProjectileEntity> Projectiles => _projectiles;
        public int Count => _projectiles.Count;

        public bool Register(ProjectileEntity projectile)
        {
            if (projectile == null)
            {
                throw new ArgumentNullException(nameof(projectile));
            }

            if (_projectiles.Contains(projectile))
            {
                return false;
            }

            _projectiles.Add(projectile);
            return true;
        }

        public bool Unregister(ProjectileEntity projectile)
        {
            if (projectile == null)
            {
                throw new ArgumentNullException(nameof(projectile));
            }

            return _projectiles.Remove(projectile);
        }

        public void Clear()
        {
            _projectiles.Clear();
        }
    }
}
