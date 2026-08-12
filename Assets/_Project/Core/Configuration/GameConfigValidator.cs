using System;

namespace Game.Core.Configuration
{
    public sealed class GameConfigValidator
    {
        public void Validate(PlayerConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            EnsurePositive(config.MaxHealth,
                nameof(config.MaxHealth));

            EnsurePositive(config.ThrustAcceleration,
                nameof(config.ThrustAcceleration));

            EnsurePositive(config.BrakingAcceleration,
                nameof(config.BrakingAcceleration));

            EnsurePositive(config.MaxSpeed,
                nameof(config.MaxSpeed));

            EnsurePositive(
                config.TurnSpeedDegreesPerSecond,
                nameof(config.TurnSpeedDegreesPerSecond));

            EnsurePositive(
                config.CollisionRadius,
                nameof(config.CollisionRadius));

            EnsurePositive(config.Mass,
                nameof(config.Mass));

            EnsurePositive(config.BulletCollisionRadius,
                nameof(config.BulletCollisionRadius));

            EnsurePositive(config.BulletMass, nameof(config.BulletMass));

            EnsurePositive(
                config.MaxActiveBullets,
                nameof(config.MaxActiveBullets));

            EnsurePositive(config.BulletSpeed,
                nameof(config.BulletSpeed));

            EnsurePositive(config.BulletDamage,
                nameof(config.BulletDamage));

            EnsurePositive(config.ShotsPerSecond,
                nameof(config.ShotsPerSecond));

            EnsurePositive(
                config.BulletLifetimeSeconds,
                nameof(config.BulletLifetimeSeconds));

            EnsurePositive(
                config.LaserMaxCharges,
                nameof(config.LaserMaxCharges));

            EnsurePositive(
                config.LaserRechargeSeconds,
                nameof(config.LaserRechargeSeconds));

            EnsurePositive(config.LaserLength,
                nameof(config.LaserLength));

            EnsurePositive(
                config.InvulnerabilitySeconds,
                nameof(config.InvulnerabilitySeconds));
        }

        public void Validate(EnemyConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            ValidateEnemyParameters(
                config.LargeAsteroid,
                nameof(config.LargeAsteroid));

            ValidateEnemyParameters(
                config.Fragment,
                nameof(config.Fragment));

            ValidateEnemyParameters(
                config.Ufo,
                nameof(config.Ufo));

            EnsurePositive(config.MinimumAsteroidSpawnIntervalSeconds,
                nameof(config.MinimumAsteroidSpawnIntervalSeconds));

            EnsureNonNegative(config.AsteroidSpawnIntervalReductionPerMinute,
                nameof(config.AsteroidSpawnIntervalReductionPerMinute));

            EnsurePositive(config.FragmentCount, nameof(config.FragmentCount));

            EnsurePositive(
                config.FragmentSpreadDegrees,
                nameof(config.FragmentSpreadDegrees));

            EnsurePositive(
                config.AsteroidSpawnIntervalSeconds,
                nameof(config.AsteroidSpawnIntervalSeconds));

            EnsurePositive(
                config.UfoSpawnIntervalSeconds,
                nameof(config.UfoSpawnIntervalSeconds));

            if (config.Fragment.Speed <= config.LargeAsteroid.Speed)
            {
                throw new ArgumentException(
                    "Fragment speed must be greater than large asteroid speed",
                    nameof(config.Fragment));
            }

            if (config.Fragment.CollisionRadius >= config.LargeAsteroid.CollisionRadius)
            {
                throw new ArgumentException(
                    "Fragment collision radius must be smaller than large asteroid collision radius",
                    nameof(config.Fragment));
            }
        }

        public void Validate(WorldConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            EnsurePositive(config.Width, nameof(config.Width));
            EnsurePositive(config.Height, nameof(config.Height));
            EnsurePositive(config.MaxEnemies, nameof(config.MaxEnemies));

            EnsurePositive(
                config.SpawnOutsideOffset,
                nameof(config.SpawnOutsideOffset));

            EnsureNonNegative(
                config.InitialAsteroidPoolSize,
                nameof(config.InitialAsteroidPoolSize));

            EnsureNonNegative(
                config.InitialUfoPoolSize,
                nameof(config.InitialUfoPoolSize));

            EnsureNonNegative(
                config.InitialProjectilePoolSize,
                nameof(config.InitialProjectilePoolSize));

            EnsureNonNegative(config.InitialCollisionVfxPoolSize, nameof(config.InitialCollisionVfxPoolSize));
        }

        private static void EnsurePositive(int value, string parameterName)
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(parameterName, value, "Value must be greater than zero");
            }
        }

        private static void EnsurePositive(float value, string parameterName)
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(parameterName, value, "Value must be greater than zero");
            }
        }

        private static void ValidateEnemyParameters(
            EnemyParameters parameters,
            string parameterName)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(parameterName);
            }

            EnsurePositive(
                parameters.MaxHealth,
                $"{parameterName}.{nameof(parameters.MaxHealth)}");

            EnsurePositive(
                parameters.Speed,
                $"{parameterName}.{nameof(parameters.Speed)}");

            EnsurePositive(
                parameters.CollisionRadius,
                $"{parameterName}.{nameof(parameters.CollisionRadius)}");

            EnsurePositive(
                parameters.Mass,
                $"{parameterName}.{nameof(parameters.Mass)}");

            EnsurePositive(
                parameters.ScoreReward,
                $"{parameterName}.{nameof(parameters.ScoreReward)}");
        }

        private static void EnsureNonNegative(float value, string parameterName)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(
                    parameterName,
                    value,
                    "Value cannot be negative");
            }
        }

        private static void EnsureNonNegative(int value, string parameterName)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(
                    parameterName,
                    value,
                    "Value cannot be negative");
            }
        }
    }
}
