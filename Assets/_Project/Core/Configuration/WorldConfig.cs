namespace Game.Core.Configuration
{
    public sealed class WorldConfig
    {
        public float Width { get; set; }
        public float Height { get; set; }

        public int MaxEnemies { get; set; }
        public float SpawnOutsideOffset { get; set; }

        public int InitialAsteroidPoolSize { get; set; }
        public int InitialUfoPoolSize { get; set; }
        public int InitialProjectilePoolSize { get; set; }
    }
}
