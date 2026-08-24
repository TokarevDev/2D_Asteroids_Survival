using System;
using Game.Core.Configuration;
using Game.Core.Enemies;
using Game.Gameplay.Combat;
using Game.Gameplay.Enemies;
using UnityEngine;
using Zenject;

namespace Game.Gameplay.Asteroids
{
    public sealed class AsteroidPool : EnemyPool<Asteroid>
    {
        public event Action<EnemyType, DeathSource> AsteroidDied;

        private const int AsteroidSortingOrderBase = 100;

        [SerializeField] private Asteroid _asteroidPrefab;
        [SerializeField] private AsteroidConfig _fragmentConfig;

        private IGameConfigProvider _configProvider;

        [Inject]
        private void Construct(IGameConfigProvider configProvider)
        {
            _configProvider = configProvider ?? throw new ArgumentNullException(nameof(configProvider));
        }

        private void Awake()
        {
            if (!ValidateSerializedReferences())
            {
                enabled = false;
                return;
            }

            InitializePool(
                InstantiateAsteroid,
                _configProvider.World.InitialAsteroidPoolSize,
                AsteroidSortingOrderBase);
        }

        protected override EnemyPhysicsView GetPhysicsView(Asteroid asteroid)
        {
            return asteroid.PhysicsView;
        }

        protected override void SetSortingOrder(Asteroid asteroid, int sortingOrder)
        {
            asteroid.SetSortingOrder(sortingOrder);
        }

        protected override void SubscribeToDeath(Asteroid asteroid)
        {
            asteroid.Died += OnAsteroidDied;
        }

        protected override void UnsubscribeFromDeath(Asteroid asteroid)
        {
            asteroid.Died -= OnAsteroidDied;
        }

        protected override void PrepareForReturn(Asteroid asteroid)
        {
            asteroid.Stop();
        }

        public Asteroid GetFragment(Vector2 position, Vector2 velocity)
        {
            EnemyParameters parameters = _configProvider.Enemy.GetParameters(EnemyType.Fragment);

            return Get(_fragmentConfig, EnemyType.Fragment, position, velocity, parameters.MaxHealth);
        }

        public Asteroid Get(AsteroidConfig config, EnemyType type, Vector2 position, Vector2 velocity, int maxHealth)
        {
            Asteroid asteroid = RentEnemy();

            try
            {
                asteroid.Initialize(config, maxHealth);
                ActivateEnemy(asteroid, type, position, velocity);
            }
            catch
            {
                Return(asteroid);
                throw;
            }

            return asteroid;
        }

        private Asteroid InstantiateAsteroid()
        {
            return Instantiate(_asteroidPrefab, transform);
        }

        private void OnAsteroidDied(Asteroid asteroid, DeathSource deathSource)
        {
            EnemyType enemyType = asteroid.PhysicsView.Entity.Type;

            Return(asteroid);

            AsteroidDied?.Invoke(enemyType, deathSource);
        }

        private bool ValidateSerializedReferences()
        {
            bool isValid = true;

            if (_asteroidPrefab == null)
            {
                Debug.LogError("Asteroid prefab reference is missing", this);
                isValid = false;
            }

            if (_fragmentConfig == null)
            {
                Debug.LogError("Fragment asteroid config reference is missing", this);
                isValid = false;
            }

            return isValid;
        }
    }
}
