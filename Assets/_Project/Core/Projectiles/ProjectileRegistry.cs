using System;
using System.Collections.Generic;

namespace Game.Core.Projectiles
{
    public sealed class ProjectileRegistry
    {
        private readonly List<ProjectileEntity> _projectiles = new();
        private readonly HashSet<ProjectileEntity> _registeredProjectiles = new();

        public IReadOnlyList<ProjectileEntity> Projectiles => _projectiles;
        public int Count => _projectiles.Count;

        public bool Register(ProjectileEntity projectile)
        {
            if (projectile == null)
            {
                throw new ArgumentNullException(nameof(projectile));
            }

            if (!_registeredProjectiles.Add(projectile))
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

            if (!_registeredProjectiles.Remove(projectile))
            {
                return false;
            }

            _projectiles.Remove(projectile);
            return true;
        }

        public void Clear()
        {
            _registeredProjectiles.Clear();
            _projectiles.Clear();
        }
    }
}
