using System;
using Game.Core.Input;
using UnityEngine;

namespace Game.Infrastructure
{
    public sealed class InputReader : IInputReader
    {
        private readonly IPlayerInputStrategy _inputStrategy;

        public InputReader(IPlayerInputStrategy inputStrategy)
        {
            _inputStrategy = inputStrategy ?? throw new ArgumentNullException(nameof(inputStrategy));
        }

        public Vector2 MoveDirection
        {
            get
            {
                PlayerInputState inputState = _inputStrategy.Read();

                return new Vector2(inputState.Turn, inputState.Thrust - inputState.Brake);
            }
        }
    }
}
