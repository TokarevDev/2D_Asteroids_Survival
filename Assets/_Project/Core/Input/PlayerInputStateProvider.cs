using System;
using UnityEngine;

namespace Game.Core.Input
{
    public sealed class PlayerInputStateProvider
    {
        private readonly IPlayerInputStrategy _inputStrategy;

        private PlayerInputState _current;
        private int _cachedFrame = -1;

        public PlayerInputState Current
        {
            get
            {
                int currentFrame = Time.frameCount;

                if (_cachedFrame != currentFrame)
                {
                    _cachedFrame = currentFrame;
                    _current = _inputStrategy.Read();
                }

                return _current;
            }
        }

        public PlayerInputStateProvider(IPlayerInputStrategy inputStrategy)
        {
            _inputStrategy = inputStrategy ?? throw new ArgumentNullException(nameof(inputStrategy));
        }
    }
}
