using System;
using Game.Core.Enemies;

namespace Game.Core.Configuration
{
    public sealed class EnemyConfig
    {
        public EnemyParameters LargeAsteroid { get; set; }
        public EnemyParameters Fragment { get; set; }
        public EnemyParameters Ufo { get; set; }

        public int FragmentCount { get; set; }
        public float FragmentSpreadDegrees { get; set; }

        public float AsteroidSpawnIntervalSeconds { get; set; }
        public float UfoSpawnIntervalSeconds { get; set; }

        public EnemyParameters GetParameters(EnemyType type)
        {
            switch (type)
            {
                case EnemyType.LargeAsteroid:
                    return LargeAsteroid;
                case EnemyType.Fragment:
                    return Fragment;
                case EnemyType.Ufo:
                    return Ufo;
                default: throw new ArgumentOutOfRangeException(nameof(type), type, "Unsupported enemy type");
            }
        }
    }
}
