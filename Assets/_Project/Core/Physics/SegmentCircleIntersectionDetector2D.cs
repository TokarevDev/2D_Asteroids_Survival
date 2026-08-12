using System;
using UnityEngine;

namespace Game.Core.Physics
{
    public sealed class SegmentCircleIntersectionDetector2D
    {
        public bool Intersects(Vector2 segmentStart, Vector2 segmentEnd, Vector2 circleCenter, float circleRadius)
        {
            if (circleRadius <= 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(circleRadius), circleRadius,
                    "Circle radius must be greater than zero");
            }

            Vector2 segment = segmentEnd - segmentStart;
            float segmentLengthSquared = segment.sqrMagnitude;

            if (segmentLengthSquared <= Mathf.Epsilon)
            {
                return (circleCenter - segmentStart).sqrMagnitude <= circleRadius * circleRadius;
            }

            float projection = Vector2.Dot(circleCenter - segmentStart, segment) / segmentLengthSquared;

            float normalizedDistance = Mathf.Clamp01(projection);
            Vector2 closestPoint = segmentStart + segment * normalizedDistance;

            return (circleCenter - closestPoint).sqrMagnitude <= circleRadius * circleRadius;
        }
    }
}
