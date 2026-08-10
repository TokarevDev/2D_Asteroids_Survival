using System;
using Game.Core.Physics;
using Game.Core.Projectiles;
using UnityEngine;

namespace Game.Gameplay.Projectiles
{
    [DisallowMultipleComponent]
    public sealed class ProjectilePhysicsView : MonoBehaviour
    {
        private Transform _cachedTransform;
        private ProjectileEntity _entity;

        public bool IsBound => _entity != null;

        public ProjectileEntity Entity =>
            _entity ?? throw new InvalidOperationException("Projectile physics view is not bound");

        private void Awake()
        {
            _cachedTransform = transform;
        }

        public void Bind(ProjectileEntity entity)
        {
            if (_entity != null)
            {
                throw new InvalidOperationException("Projectile physics view is already bound");
            }

            _entity = entity ?? throw new ArgumentNullException(nameof(entity));

            Synchronize();
        }

        public void Synchronize()
        {
            CustomPhysicsBody2D body = Entity.PhysicsBody;
            Vector3 position = _cachedTransform.position;

            position.x = body.Position.x;
            position.y = body.Position.y;

            _cachedTransform.SetPositionAndRotation(position, Quaternion.Euler(0f, 0f, body.RotationDegrees));
        }

        public void Unbind()
        {
            _entity = null;
        }
    }
}
