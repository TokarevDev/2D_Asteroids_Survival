using Game.Core.Physics;
using UnityEngine;

namespace Game.Core.Enemies
{
    public sealed class EnemyEntity
    {
        public EnemyType Type { get; private set; }
        public CustomPhysicsBody2D PhysicsBody { get; }

        public EnemyEntity(EnemyType type, Vector2 position, Vector2 velocity, float rotationDegrees,
            float collisionRadius, float mass)
        {
            Type = type;
            PhysicsBody = new CustomPhysicsBody2D(position, velocity, rotationDegrees, collisionRadius, mass);
        }

        public void Reset(EnemyType type, Vector2 position, Vector2 velocity, float rotationDegrees,
            float collisionRadius, float mass)
        {
            Type = type;
            PhysicsBody.Reset(position, velocity, rotationDegrees, collisionRadius, mass);
        }
    }
}
