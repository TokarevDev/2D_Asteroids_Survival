using System;
using Game.Core.Physics;
using Game.Core.World;
using UnityEngine;

namespace Game.Core.Enemies
{
    public sealed class UfoPursuitMovement
    {
        private const float MinimumDisplacementSqrMagnitude = 0.0001f;

        private readonly ToroidalWorld2D _world;
        private readonly float _speed;

        public UfoPursuitMovement(ToroidalWorld2D world, float speed)
        {
            _world = world ?? throw new ArgumentNullException(nameof(world));

            if (speed <= 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(speed), speed, "UFO speed must be greater than zero");
            }

            _speed = speed;
        }

        public void Step(CustomPhysicsBody2D body, Vector2 targetPosition)
        {
            if (body == null)
            {
                throw new ArgumentNullException(nameof(body));
            }

            Vector2 displacement = _world.GetShortestDisplacement(body.Position, targetPosition);

            if (displacement.sqrMagnitude < MinimumDisplacementSqrMagnitude)
            {
                body.SetVelocity(Vector2.zero);
                return;
            }

            body.SetVelocity(displacement.normalized * _speed);
        }
    }
}
