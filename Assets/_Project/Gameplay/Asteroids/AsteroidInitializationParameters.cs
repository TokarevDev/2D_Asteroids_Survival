using System;

namespace Game.Gameplay.Asteroids
{
    public readonly struct AsteroidInitializationParameters
    {
        public AsteroidConfig Config { get; }
        public int MaxHealth { get; }

        public AsteroidInitializationParameters(AsteroidConfig config, int maxHealth)
        {
            Config = config ?? throw new ArgumentNullException(nameof(config));

            MaxHealth = maxHealth;
        }
    }
}
