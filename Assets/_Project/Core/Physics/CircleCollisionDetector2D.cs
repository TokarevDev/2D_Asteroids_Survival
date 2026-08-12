using System;
using UnityEngine;

namespace Game.Core.Physics
{
    public sealed class CircleCollisionDetector2D
    {
        public bool Intersects(CustomPhysicsBody2D first, CustomPhysicsBody2D second)
        {
            ValidateBodies(first, second);

            Vector2 displacement = second.Position - first.Position;

            return Intersects(first, second, displacement);
        }

        public bool Intersects(CustomPhysicsBody2D first, CustomPhysicsBody2D second, Vector2 displacement)
        {
            ValidateBodies(first, second);

            float combinedRadius = first.CollisionRadius + second.CollisionRadius;

            return displacement.sqrMagnitude <= combinedRadius * combinedRadius;
        }

        private static void ValidateBodies(CustomPhysicsBody2D first, CustomPhysicsBody2D second)
        {
            if (first == null)
            {
                throw new ArgumentNullException(nameof(first));
            }

            if (second == null)
            {
                throw new ArgumentNullException(nameof(second));
            }
        }
    }
}
