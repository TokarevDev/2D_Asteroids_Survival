using System;
using UnityEngine;

namespace Game.Core.Physics
{
    public sealed class CustomPhysicsBody2D
    {
        public Vector2 Position { get; private set; }
        public Vector2 Velocity { get; private set; }
        public float RotationDegrees { get; private set; }
        public float CollisionRadius { get; private set; }
        public float Mass { get; private set; }

        public CustomPhysicsBody2D(Vector2 position, Vector2 velocity, float rotationDegrees, float collisionRadius,
            float mass)
        {
            Reset(position, velocity, rotationDegrees, collisionRadius, mass);
        }

        internal void SetPosition(Vector2 position)
        {
            Position = position;
        }

        public void Reset(Vector2 position, Vector2 velocity, float rotationDegrees, float collisionRadius, float mass)
        {
            if (collisionRadius <= 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(collisionRadius), collisionRadius,
                    "Collision radius must be greater than zero");
            }

            if (mass <= 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(mass), mass, "Mass must be greater than zero");
            }

            Position = position;
            Velocity = velocity;
            RotationDegrees = rotationDegrees;
            CollisionRadius = collisionRadius;
            Mass = mass;
        }
    }
}
