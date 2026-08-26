using System;
using System.Collections.Generic;
using Game.Core.Enemies;

namespace Game.Core.Configuration
{
    public sealed class EnemyConfig
    {
        private Dictionary<EnemyType, EnemyParameters> _parametersByType;

        public Dictionary<string, EnemyParameters> ParametersByKey { get; set; }

        public IReadOnlyDictionary<EnemyType, EnemyParameters>
            ParametersByType =>
            _parametersByType
            ?? throw new InvalidOperationException(
                "Enemy parameter lookup is not initialized");

        public int FragmentCount { get; set; }
        public float FragmentSpreadDegrees { get; set; }

        public float AsteroidSpawnIntervalSeconds { get; set; }
        public float MinimumAsteroidSpawnIntervalSeconds { get; set; }
        public float AsteroidSpawnIntervalReductionPerMinute { get; set; }
        public float UfoSpawnIntervalSeconds { get; set; }

        public void InitializeParameterLookup()
        {
            if (_parametersByType != null)
            {
                return;
            }

            if (ParametersByKey == null)
            {
                throw new ArgumentNullException(nameof(ParametersByKey));
            }

            var lookup =
                new Dictionary<EnemyType, EnemyParameters>(
                    ParametersByKey.Count);

            foreach (KeyValuePair<string, EnemyParameters> entry
                     in ParametersByKey)
            {
                if (!TryParseExactEnemyType(
                        entry.Key,
                        out EnemyType type))
                {
                    throw new ArgumentException(
                        $"Unsupported enemy type key '{entry.Key}'",
                        nameof(ParametersByKey));
                }

                if (entry.Value == null)
                {
                    throw new ArgumentNullException(
                        $"{nameof(ParametersByKey)}[{entry.Key}]");
                }

                if (!lookup.TryAdd(type, entry.Value))
                {
                    throw new ArgumentException(
                        $"Duplicate enemy type '{type}'",
                        nameof(ParametersByKey));
                }
            }

            foreach (EnemyType requiredType
                     in Enum.GetValues(typeof(EnemyType)))
            {
                if (!lookup.ContainsKey(requiredType))
                {
                    throw new ArgumentException(
                        $"Parameters for enemy type '{requiredType}' are missing",
                        nameof(ParametersByKey));
                }
            }

            _parametersByType = lookup;
        }

        public EnemyParameters GetParameters(EnemyType type)
        {
            if (!ParametersByType.TryGetValue(
                    type,
                    out EnemyParameters parameters))
            {
                throw new KeyNotFoundException(
                    $"Parameters for enemy type '{type}' are not configured");
            }

            return parameters;
        }

        private static bool TryParseExactEnemyType(
            string key,
            out EnemyType type)
        {
            return Enum.TryParse(key, false, out type)
                   && Enum.IsDefined(typeof(EnemyType), type)
                   && string.Equals(
                       key,
                       type.ToString(),
                       StringComparison.Ordinal);
        }
    }
}
