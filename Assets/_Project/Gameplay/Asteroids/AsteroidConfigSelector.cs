using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Game.Gameplay.Asteroids
{
    public sealed class AsteroidConfigSelector
    {
        private readonly IReadOnlyList<AsteroidConfig> _configs;
        private readonly int[] _configOrder;

        private int _nextConfigIndex;
        private int _lastConfigIndex = -1;

        public AsteroidConfigSelector(IReadOnlyList<AsteroidConfig> configs)
        {
            _configs = configs ?? throw new ArgumentNullException(nameof(configs));

            if (configs.Count == 0)
            {
                throw new ArgumentException("At least one asteroid config is required", nameof(configs));
            }

            _configOrder = new int[configs.Count];
            _nextConfigIndex = _configOrder.Length;
        }

        public AsteroidConfig GetNextConfig()
        {
            if (_nextConfigIndex >= _configOrder.Length)
            {
                ShuffleConfigs();
            }

            int configIndex = _configOrder[_nextConfigIndex++];
            _lastConfigIndex = configIndex;

            return _configs[configIndex];
        }

        private void ShuffleConfigs()
        {
            for (int i = 0; i < _configOrder.Length; i++)
            {
                _configOrder[i] = i;
            }

            for (int i = _configOrder.Length - 1; i > 0; i--)
            {
                int randomIndex = Random.Range(0, i + 1);
                (_configOrder[i], _configOrder[randomIndex]) = (_configOrder[randomIndex], _configOrder[i]);
            }

            if (_configOrder.Length > 1 && _configOrder[0] == _lastConfigIndex)
            {
                int swapIndex = Random.Range(1, _configOrder.Length);

                (_configOrder[0], _configOrder[swapIndex]) = (_configOrder[swapIndex], _configOrder[0]);
            }

            _nextConfigIndex = 0;
        }
    }
}
