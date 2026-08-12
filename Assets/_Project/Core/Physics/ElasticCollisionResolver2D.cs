using System;
using Vector2 = UnityEngine.Vector2;

namespace Game.Core.Physics
{
    public sealed class ElasticCollisionResolver2D
    {
        private const float MinimumDisplacementSqrMagnitude = 0.0001f;

        public void Resolve(CustomPhysicsBody2D first, CustomPhysicsBody2D second, Vector2 displacement)
        {
            if (first == null)
            {
                throw new ArgumentNullException(nameof(first));
            }

            if (second == null)
            {
                throw new ArgumentNullException(nameof(second));
            }

            Vector2 normal = GetCollisionNormal(first, second, displacement);

            SeparateBodies(first, second, displacement.magnitude, normal);
            ApplyImpulse(first, second, normal);
        }

        private static Vector2 GetCollisionNormal(CustomPhysicsBody2D first, CustomPhysicsBody2D second,
            Vector2 displacement)
        {
            if (displacement.sqrMagnitude >= MinimumDisplacementSqrMagnitude)
            {
                return displacement.normalized;
            }

            Vector2 relativeVelocity = second.Velocity - first.Velocity;

            if (relativeVelocity.sqrMagnitude >= MinimumDisplacementSqrMagnitude)
            {
                return relativeVelocity.normalized;
            }

            return Vector2.right;
        }

        private static void SeparateBodies(CustomPhysicsBody2D first, CustomPhysicsBody2D second,
            float distance, Vector2 normal)
        {
            float penetration = first.CollisionRadius + second.CollisionRadius - distance;

            if (penetration <= 0f)
            {
                return;
            }

            float firstInverseMass = 1f / first.Mass;
            float secondInverseMass = 1f / second.Mass;
            float inverseMassSum = firstInverseMass + secondInverseMass;

            first.SetPosition(first.Position - normal * penetration * firstInverseMass / inverseMassSum);

            second.SetPosition(second.Position + normal * penetration * secondInverseMass / inverseMassSum);
        }

        private static void ApplyImpulse(CustomPhysicsBody2D first, CustomPhysicsBody2D second, Vector2 normal)
        {
            Vector2 relativeVelocity = second.Velocity - first.Velocity;
            float velocityAlongNormal = Vector2.Dot(relativeVelocity, normal);

            if (velocityAlongNormal >= 0f)
            {
                return;
            }

            float firstInverseMass = 1f / first.Mass;
            float secondInverseMass = 1f / second.Mass;

            float impulseMagnitude = -2f * velocityAlongNormal / (firstInverseMass + secondInverseMass);

            Vector2 impulse = normal * impulseMagnitude;

            first.SetVelocity(first.Velocity - impulse * firstInverseMass);

            second.SetVelocity(second.Velocity + impulse * secondInverseMass);
        }
    }
}
