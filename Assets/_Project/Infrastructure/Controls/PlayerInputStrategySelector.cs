using System;
using Game.Core.Input;

namespace Game.Infrastructure.Controls
{
    public sealed class PlayerInputStrategySelector : IPlayerInputStrategy
    {
        private readonly IPlayerInputStrategy _selectedStrategy;

        public PlayerInputStrategySelector(MobileInputStrategy mobileInputStrategy,
            KeyboardMouseInputStrategy keyboardMouseInputStrategy)
        {
            if (mobileInputStrategy == null)
            {
                throw new ArgumentNullException(nameof(mobileInputStrategy));
            }

            if (keyboardMouseInputStrategy == null)
            {
                throw new ArgumentNullException(nameof(keyboardMouseInputStrategy));
            }

            _selectedStrategy = UnityEngine.Application.isMobilePlatform
                ? mobileInputStrategy
                : keyboardMouseInputStrategy;
        }

        public PlayerInputState Read()
        {
            return _selectedStrategy.Read();
        }
    }
}
