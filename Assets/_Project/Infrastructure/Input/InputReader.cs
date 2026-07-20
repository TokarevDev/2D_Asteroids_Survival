using System;
using UnityEngine;
using Game.Core;
using Zenject;

namespace Game.Infrastructure
{
    public class InputReader : IInputReader, IInitializable, IDisposable
    {
        private readonly PlayerInputActions _inputActions = new();

        public Vector2 MoveDirection =>
            _inputActions.Player.Move.ReadValue<Vector2>();


        public void Initialize()
        {
            _inputActions.Player.Enable();
        }

        public void Dispose()
        {
            _inputActions.Player.Disable();
            _inputActions.Dispose();
        }
    }
}
