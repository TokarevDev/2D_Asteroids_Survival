using System;
using Game.Core.Physics;
using Game.Gameplay.Player;
using UnityEngine;
using Zenject;

namespace Game.UI.HUD
{
    public sealed class PlayerTelemetryViewModel : ITickable
    {
        private const float FullRotationDegrees = 360f;

        public event Action<Vector2, float, float> TelemetryChanged;

        private readonly PlayerPhysicsController _playerPhysicsController;

        public Vector2 Position { get; private set; }
        public float RotationDegrees { get; private set; }
        public float Speed { get; private set; }

        public PlayerTelemetryViewModel(PlayerPhysicsController playerPhysicsController)
        {
            _playerPhysicsController = playerPhysicsController ??
                                       throw new ArgumentNullException(nameof(playerPhysicsController));
        }

        public void Tick()
        {
            CustomPhysicsBody2D body = _playerPhysicsController.Body;

            Position = body.Position;
            RotationDegrees = Mathf.Repeat(body.RotationDegrees, FullRotationDegrees);
            Speed = body.Velocity.magnitude;

            TelemetryChanged?.Invoke(Position, RotationDegrees, Speed);
        }
    }
}
