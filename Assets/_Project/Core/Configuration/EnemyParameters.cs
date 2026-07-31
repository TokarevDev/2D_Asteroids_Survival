namespace Game.Core.Configuration
{
    public sealed class EnemyParameters
    {
        public int MaxHealth { get; set; }
        public float Speed { get; set; }
        public float CollisionRadius { get; set; }
        public float Mass { get; set; }
        public int ScoreReward { get; set; }
    }
}
