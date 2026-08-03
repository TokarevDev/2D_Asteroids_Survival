using Game.Core.Input;
using UnityEngine;

namespace Game.Infrastructure.Controls
{
    public sealed class KeyboardMouseInputStrategy : IPlayerInputStrategy
    {
        public PlayerInputState Read()
        {
            float turn = ReadTurn();
            float thrust = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) ? 1f : 0f;
            float brake = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) ? 1f : 0f;
            bool fireBulletPressed = Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0);
            bool fireLaserPressed = Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(1);

            return new PlayerInputState(turn, thrust, brake, fireBulletPressed, fireLaserPressed);
        }

        private static float ReadTurn()
        {
            bool turnLeft = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
            bool turnRight = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);

            if (turnLeft == turnRight)
            {
                return 0f;
            }

            return turnLeft ? -1f : 1f;
        }
    }
}
