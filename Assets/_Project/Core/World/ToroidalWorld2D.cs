using System;
using Game.Core.Physics;
using UnityEngine;

namespace Game.Core.World
{
    public sealed class ToroidalWorld2D
    {
        private readonly float _width;
        private readonly float _height;
        private readonly float _halfWidth;
        private readonly float _halfHeight;

        public ToroidalWorld2D(float width, float height)
        {
            if (width <= 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(width), width, "World width must be greater than zero");
            }

            if (height <= 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(height), height, "World height must be greater than zero");
            }

            _width = width;
            _height = height;
            _halfWidth = width * 0.5f;
            _halfHeight = height * 0.5f;
        }

        public void Wrap(CustomPhysicsBody2D body)
        {
            if (body == null)
            {
                throw new ArgumentNullException(nameof(body));
            }

            Vector2 position = body.Position;

            float wrappedX = WrapCoordinate(position.x, _halfWidth, _width);
            float wrappedY = WrapCoordinate(position.y, _halfHeight, _height);

            if (wrappedX == position.x && wrappedY == position.y)
            {
                return;
            }

            body.SetPosition(new Vector2(wrappedX, wrappedY));
        }

        private static float WrapCoordinate(float coordinate, float halfExtent, float size)
        {
            if (coordinate >= -halfExtent && coordinate < halfExtent)
            {
                return coordinate;
            }

            return Mathf.Repeat(coordinate + halfExtent, size) - halfExtent;
        }
    }
}
