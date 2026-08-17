using Game.Core.Input;
using UnityEngine;
using UnityInput = UnityEngine.Input;

namespace Game.Infrastructure.Controls
{
    public sealed class KeyboardMouseInputStrategy : IPlayerInputStrategy
    {
        public PlayerInputState Read()
        {
            float turn = ReadTurn();
            float thrust = UnityInput.GetKey(KeyCode.W) || UnityInput.GetKey(KeyCode.UpArrow) ? 1f : 0f;
            float brake = UnityInput.GetKey(KeyCode.S) || UnityInput.GetKey(KeyCode.DownArrow) ? 1f : 0f;
            bool fireBulletHeld = UnityInput.GetKey(KeyCode.Space) || UnityInput.GetMouseButton(0);
            bool fireLaserPressed = UnityInput.GetKeyDown(KeyCode.E) || UnityInput.GetMouseButtonDown(1);

            return new PlayerInputState(turn, thrust, brake, fireBulletHeld, fireLaserPressed);
        }

        private static float ReadTurn()
        {
            bool turnLeft = UnityInput.GetKey(KeyCode.A) || UnityInput.GetKey(KeyCode.LeftArrow);
            bool turnRight = UnityInput.GetKey(KeyCode.D) || UnityInput.GetKey(KeyCode.RightArrow);

            if (turnLeft == turnRight)
            {
                return 0f;
            }

            return turnLeft ? -1f : 1f;
        }
    }
}
