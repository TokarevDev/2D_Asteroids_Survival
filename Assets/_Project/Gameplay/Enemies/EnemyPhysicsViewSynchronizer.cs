using System;
using System.Collections.Generic;
using Zenject;

namespace Game.Gameplay.Enemies
{
    public sealed class EnemyPhysicsViewSynchronizer : IFixedTickable
    {
        private readonly List<EnemyPhysicsView> _views = new();

        public int ViewCount => _views.Count;

        public bool Register(EnemyPhysicsView view)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            if (!view.IsBound)
            {
                throw new InvalidOperationException("Cannot register an unbound enemy physics view");
            }

            if (_views.Contains(view))
            {
                return false;
            }

            _views.Add(view);
            return true;
        }

        public bool Unregister(EnemyPhysicsView view)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            return _views.Remove(view);
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
