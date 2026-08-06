using System;
using Game.Core.Physics;
using UnityEngine;

namespace Game.Core.Projectiles
{
    public sealed class ProjectileEntity
    {
        public CustomPhysicsBody2D PhysicsBody { get; }

        public int Damage { get; private set; }
        public float RemainingLifetimeSeconds { get; private set; }

        public bool IsExpired => RemainingLifetimeSeconds <= 0f;

        public ProjectileEntity(Vector2 position, Vector2 velocity, float rotationDegrees, float collisionRadius,
            float mass, int damage, float lifetimeSeconds)
        {
            ValidateCombatParameters(damage, lifetimeSeconds);

            PhysicsBody = new CustomPhysicsBody2D(position, velocity, rotationDegrees, collisionRadius, mass);

            Damage = damage;
            RemainingLifetimeSeconds = lifetimeSeconds;
        }

        public void Reset(Vector2 position, Vector2 velocity, float rotationDegrees, float collisionRadius, float mass,
            int damage, float lifetimeSeconds)
        {
            ValidateCombatParameters(damage, lifetimeSeconds);

            PhysicsBody.Reset(position, velocity, rotationDegrees, collisionRadius, mass);

            Damage = damage;
            RemainingLifetimeSeconds = lifetimeSeconds;
        }

        public void AdvanceLifetime(float deltaTime)
        {
            if (deltaTime < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(deltaTime), deltaTime, "Delta time cannot be negative");
            }

            RemainingLifetimeSeconds = Mathf.Max(0f, RemainingLifetimeSeconds - deltaTime);
        }

        private static void ValidateCombatParameters(int damage, float lifetimeSeconds)
        {
            if (damage <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(damage));
            }

            if (lifetimeSeconds <= 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(lifetimeSeconds));
            }
        }
    }
}
