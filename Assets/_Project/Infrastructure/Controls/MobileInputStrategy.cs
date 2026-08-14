using System;
using Game.Core.Input;
using UnityEngine;

namespace Game.Infrastructure.Controls
{
    public sealed class MobileInputStrategy : IPlayerInputStrategy
    {
        private readonly MobileInputBuffer _inputBuffer;

        private PlayerInputState _cachedInput;
        private int _cachedFrame = -1;

        public MobileInputStrategy(MobileInputBuffer inputBuffer)
        {
            _inputBuffer = inputBuffer ?? throw new ArgumentNullException(nameof(inputBuffer));
        }

        public PlayerInputState Read()
        {
            int currentFrame = Time.frameCount;

            if (_cachedFrame == currentFrame)
            {
                return _cachedInput;
            }

            _cachedFrame = currentFrame;

            _cachedInput = new PlayerInputState(
                0f,
                0f,
                0f,
                _inputBuffer.MovementDirection,
                _inputBuffer.FireBulletHeld,
                _inputBuffer.ConsumeLaserFireRequest());

            return _cachedInput;
        }
    }
}
