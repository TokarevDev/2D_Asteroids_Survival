namespace Game.Core.Configuration
{
    public sealed class PlayerConfig
    {
        public int MaxHealth { get; set; }

        public float ThrustAcceleration { get; set; }
        public float BrakingAcceleration { get; set; }
        public float MaxSpeed { get; set; }
        public float TurnSpeedDegreesPerSecond { get; set; }

        public float CollisionRadius { get; set; }
        public float Mass { get; set; }

        public float BulletCollisionRadius { get; set; }
        public float BulletMass { get; set; }

        public int MaxActiveBullets { get; set; }
        public float BulletSpeed { get; set; }
        public int BulletDamage { get; set; }
        public float ShotsPerSecond { get; set; }
        public float BulletLifetimeSeconds { get; set; }

        public int LaserMaxCharges { get; set; }
        public float LaserRechargeSeconds { get; set; }
        public float LaserLength { get; set; }
        public float LaserVisualDurationSeconds { get; set; }

        public float InvulnerabilitySeconds { get; set; }
    }
}
