using System;
using Game.Core.Input;

namespace Game.Infrastructure.Controls
{
    public sealed class MobileInputStrategy : IPlayerInputStrategy
    {
        private readonly MobileInputBuffer _inputBuffer;

        public MobileInputStrategy(MobileInputBuffer inputBuffer)
        {
            _inputBuffer = inputBuffer ?? throw new ArgumentNullException(nameof(inputBuffer));
        }

        public PlayerInputState Read()
        {
            return new PlayerInputState(
                0f,
                0f,
                0f,
                _inputBuffer.MovementDirection,
                _inputBuffer.FireBulletHeld,
                _inputBuffer.ConsumeLaserFireRequest());
        }
    }
}
