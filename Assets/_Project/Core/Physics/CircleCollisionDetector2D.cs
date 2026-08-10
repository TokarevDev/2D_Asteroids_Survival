using System;
using UnityEngine;

namespace Game.Core.Physics
{
    public sealed class CircleCollisionDetector2D
    {
        public bool Intersects(CustomPhysicsBody2D first, CustomPhysicsBody2D second)
        {
            if (first == null)
            {
                throw new ArgumentNullException(nameof(first));
            }

            if (second == null)
            {
                throw new ArgumentNullException(nameof(second));
            }

            Vector2 offset = second.Position - first.Position;
            float combinedRadius = first.CollisionRadius + second.CollisionRadius;

            return offset.sqrMagnitude <= combinedRadius * combinedRadius;
        }
    }
}
