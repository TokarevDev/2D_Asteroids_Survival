using System;
using System.Collections.Generic;
using Game.Core.Projectiles;
using Zenject;

namespace Game.Gameplay.Projectiles
{
    public sealed class ProjectilePhysicsViewSynchronizer : IFixedTickable
    {
        private readonly Dictionary<ProjectileEntity, ProjectilePhysicsView> _viewsByEntity = new();

        public int ViewCount => _viewsByEntity.Count;

        public bool Register(ProjectilePhysicsView view)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            if (!view.IsBound)
            {
                throw new InvalidOperationException("Cannot register an unbound projectile physics view");
            }

            return _viewsByEntity.TryAdd(view.Entity, view);
        }

        public bool Unregister(ProjectilePhysicsView view)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            if (!view.IsBound)
            {
                return false;
            }

            ProjectileEntity entity = view.Entity;

            if (!_viewsByEntity.TryGetValue(entity, out ProjectilePhysicsView registeredView) ||
                !ReferenceEquals(registeredView, view))
            {
                return false;
            }

            return _viewsByEntity.Remove(entity);
        }

        public bool TryGetView(ProjectileEntity entity, out ProjectilePhysicsView view)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            return _viewsByEntity.TryGetValue(entity, out view);
        }

        public void FixedTick()
        {
            foreach (KeyValuePair<ProjectileEntity, ProjectilePhysicsView> pair in _viewsByEntity)
            {
                pair.Value.Synchronize();
            }
        }
    }
}
