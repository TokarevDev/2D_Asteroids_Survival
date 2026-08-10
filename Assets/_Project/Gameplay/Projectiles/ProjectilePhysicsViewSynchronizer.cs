using System;
using System.Collections.Generic;
using Game.Core.Projectiles;
using Zenject;

namespace Game.Gameplay.Projectiles
{
    public sealed class ProjectilePhysicsViewSynchronizer : IFixedTickable
    {
        private readonly List<ProjectilePhysicsView> _views = new();

        public int ViewCount => _views.Count;

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

            if (_views.Contains(view))
            {
                return false;
            }

            _views.Add(view);
            return true;
        }

        public bool Unregister(ProjectilePhysicsView view)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            return _views.Remove(view);
        }

        public bool TryGetView(ProjectileEntity entity, out ProjectilePhysicsView view)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            for (int i = 0; i < _views.Count; i++)
            {
                ProjectilePhysicsView candidate = _views[i];

                if (ReferenceEquals(candidate.Entity, entity))
                {
                    view = candidate;
                    return true;
                }
            }

            view = null;
            return false;
        }

        public void FixedTick()
        {
            for (int i = 0; i < _views.Count; i++)
            {
                _views[i].Synchronize();
            }
        }
    }
}
