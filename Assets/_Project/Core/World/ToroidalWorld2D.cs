using System;
using Game.Core.Configuration;
using Game.Core.Physics;
using UnityEngine;

namespace Game.Core.World
{
    public sealed class ToroidalWorld2D
    {
        private float _width;
        private float _height;
        private float _halfWidth;
        private float _halfHeight;

        public float HalfWidth => _halfWidth;
        public float HalfHeight => _halfHeight;

        public ToroidalWorld2D(WorldConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            SetSize(config.Width, config.Height);
        }

        public void SetSize(float width, float height)
        {
            if (width <= 0f)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(width), width, "World width must be greater than zero");
            }

            if (height <= 0f)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(height), height, "World height must be greater than zero");
            }

            _width = width;
            _height = height;
            _halfWidth = width * 0.5f;
            _halfHeight = height * 0.5f;
        }

        public bool Contains(Vector2 position)
        {
            return position.x >= -_halfWidth &&
                   position.x < _halfWidth &&
                   position.y >= -_halfHeight &&
                   position.y < _halfHeight;
        }

        public Vector2 GetShortestDisplacement(Vector2 from, Vector2 to)
        {
            float displacementX = WrapCoordinate(to.x - from.x, _halfWidth, _width);

            float displacementY = WrapCoordinate(to.y - from.y, _halfHeight, _height);

            return new Vector2(displacementX, displacementY);
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

            if (Mathf.Approximately(wrappedX, position.x) &&
                Mathf.Approximately(wrappedY, position.y))
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
