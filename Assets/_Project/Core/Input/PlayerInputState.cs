using UnityEngine;

namespace Game.Core.Input
{
    public readonly struct PlayerInputState
    {
        public float Turn { get; }
        public float Thrust { get; }
        public float Brake { get; }
        public Vector2 MovementDirection { get; }

        public bool FireBulletHeld { get; }
        public bool FireLaserPressed { get; }

        public PlayerInputState(float turn, float thrust, float brake, Vector2 movementDirection, bool fireBulletHeld,
            bool fireLaserPressed)
        {
            Turn = turn;
            Thrust = thrust;
            Brake = brake;
            MovementDirection = Vector2.ClampMagnitude(movementDirection, 1f);
            FireBulletHeld = fireBulletHeld;
            FireLaserPressed = fireLaserPressed;
        }

        public PlayerInputState(float turn, float thrust, float brake, bool fireBulletHeld, bool fireLaserPressed)
            : this(turn, thrust, brake, Vector2.zero, fireBulletHeld, fireLaserPressed)
        {
        }
    }
}
