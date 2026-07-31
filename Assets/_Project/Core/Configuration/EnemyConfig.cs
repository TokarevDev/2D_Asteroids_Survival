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
    }
}
