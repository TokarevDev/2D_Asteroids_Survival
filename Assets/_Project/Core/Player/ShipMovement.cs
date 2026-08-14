using System;
using Game.Core.Configuration;
using Game.Core.Input;
using Game.Core.Physics;
using UnityEngine;

namespace Game.Core.Player
{
    public sealed class ShipMovement
    {
        private readonly CustomPhysicsBody2D _body;
        private readonly PlayerConfig _config;

        public ShipMovement(CustomPhysicsBody2D body, PlayerConfig config)
        {
            _body = body ?? throw new ArgumentNullException(nameof(body));
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public void Step(PlayerInputState input, float deltaTime)
        {
            ValidateDeltaTime(deltaTime);

            if (input.MovementDirection.sqrMagnitude > 0f)
            {
                ApplyDirectionalMovement(input.MovementDirection, deltaTime);
                return;
            }

            ApplyRotation(input.Turn, deltaTime);
            ApplyVelocity(input.Thrust, input.Brake, deltaTime);
        }

        private void ApplyDirectionalMovement(Vector2 movementDirection, float deltaTime)
        {
            float targetRotation = Mathf.Atan2(-movementDirection.x, movementDirection.y) * Mathf.Rad2Deg;

            float nextRotation = Mathf.MoveTowardsAngle(_body.RotationDegrees, targetRotation,
                _config.TurnSpeedDegreesPerSecond * deltaTime);

            _body.SetRotation(Mathf.Repeat(nextRotation, 360f));

            ApplyVelocity(movementDirection.magnitude, 0f, deltaTime);
        }

        private void ApplyRotation(float turnInput, float deltaTime)
        {
            float clampedTurnInput = Mathf.Clamp(turnInput, -1f, 1f);

            float nextRotation =
                _body.RotationDegrees - clampedTurnInput * _config.TurnSpeedDegreesPerSecond * deltaTime;

            _body.SetRotation(Mathf.Repeat(nextRotation, 360f));
        }

        private void ApplyVelocity(float thrustInput, float brakeInput, float deltaTime)
        {
            Vector2 velocityAfterThrust = CalculateVelocityAfterThrust(thrustInput, deltaTime);

            Vector2 velocityAfterBraking =
                CalculateVelocityAfterBraking(velocityAfterThrust, brakeInput, deltaTime);

            Vector2 clampedVelocity = Vector2.ClampMagnitude(velocityAfterBraking, _config.MaxSpeed);

            _body.SetVelocity(clampedVelocity);
        }

        private Vector2 CalculateVelocityAfterThrust(float thrustInput, float deltaTime)
        {
            float clampedThrustInput = Mathf.Clamp01(thrustInput);
            Vector2 forward = CalculateForwardDirection();

            Vector2 acceleration = forward * (_config.ThrustAcceleration * clampedThrustInput);

            return _body.Velocity + acceleration * deltaTime;
        }

        private Vector2 CalculateVelocityAfterBraking(Vector2 currentVelocity, float brakeInput, float deltaTime)
        {
            float clampedBrakeInput = Mathf.Clamp01(brakeInput);

            return Vector2.MoveTowards(currentVelocity, Vector2.zero,
                _config.BrakingAcceleration * clampedBrakeInput * deltaTime);
        }

        private Vector2 CalculateForwardDirection()
        {
            float rotationRadians = _body.RotationDegrees * Mathf.Deg2Rad;

            return new Vector2(-Mathf.Sin(rotationRadians), Mathf.Cos(rotationRadians));
        }

        private static void ValidateDeltaTime(float deltaTime)
        {
            if (deltaTime < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(deltaTime), deltaTime, "Delta time cannot be negative");
            }
        }
    }
}
