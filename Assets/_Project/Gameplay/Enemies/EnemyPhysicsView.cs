using System;
using Game.Core.Enemies;
using Game.Core.Physics;
using UnityEngine;

namespace Game.Gameplay.Enemies
{
    [DisallowMultipleComponent]
    public sealed class EnemyPhysicsView : MonoBehaviour
    {
        private Transform _cachedTransform;
        private EnemyEntity _entity;

        public bool IsBound => _entity != null;

        public EnemyEntity Entity => _entity ?? throw new InvalidOperationException("Enemy physics view is not bound");

        public Vector2 Position => _cachedTransform.position;
        public float RotationDegrees => _cachedTransform.eulerAngles.z;

        private void Awake()
        {
            _cachedTransform = transform;
        }

        public void ApplyState(Vector2 position, float rotationDegrees)
        {
            Vector3 currentPosition = _cachedTransform.position;
            currentPosition.x = position.x;
            currentPosition.y = position.y;

            _cachedTransform.SetPositionAndRotation(currentPosition, Quaternion.Euler(0f, 0f, rotationDegrees));
        }

        public void Bind(EnemyEntity entity)
        {
            if (_entity != null)
            {
                throw new InvalidOperationException("Enemy physics view is already bound");
            }

            _entity = entity ?? throw new ArgumentNullException(nameof(entity));

            Synchronize();
        }

        public void Unbind()
        {
            _entity = null;
        }

        public void Synchronize()
        {
            CustomPhysicsBody2D body = Entity.PhysicsBody;

            ApplyState(body.Position, body.RotationDegrees);
        }
    }
}
