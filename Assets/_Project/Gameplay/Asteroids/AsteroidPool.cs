using System;
using Game.Core.Configuration;
using Game.Core.Enemies;
using Game.Gameplay.Enemies;
using UnityEngine;
using Zenject;

namespace Game.Gameplay
{
    public sealed class AsteroidPool : MonoBehaviour
    {
        public event Action<EnemyType, DeathSource> AsteroidDied;

        private const int AsteroidSortingOrderBase = 100;

        [SerializeField] private Asteroid _asteroidPrefab;

        private EnemyLifecycleService _enemyLifecycleService;
        private IGameConfigProvider _configProvider;

        private ObjectPool<Asteroid> _pool;
        private int _nextSortingOrder = AsteroidSortingOrderBase;

        [Inject]
        private void Construct(EnemyLifecycleService enemyLifecycleService, IGameConfigProvider configProvider)
        {
            _enemyLifecycleService =
                enemyLifecycleService ?? throw new ArgumentNullException(nameof(enemyLifecycleService));
            _configProvider = configProvider ?? throw new ArgumentNullException(nameof(configProvider));
        }

        private void Awake()
        {
            if (!ValidateSerializedReferences())
            {
                enabled = false;
                return;
            }

            _pool = new ObjectPool<Asteroid>(CreateAsteroid, _configProvider.World.InitialAsteroidPoolSize);
        }

        private void OnDestroy()
        {
            if (_pool == null)
            {
                return;
            }

            for (int i = 0; i < _pool.CreatedItems.Count; i++)
            {
                Asteroid asteroid = _pool.CreatedItems[i];
                if (asteroid == null)
                {
                    continue;
                }

                asteroid.Died -= OnAsteroidDied;
            }

            _pool.Clear();
        }

        public Asteroid Get(AsteroidConfig config, EnemyType type, Vector2 position, Vector2 velocity, int maxHealth)
        {
            Asteroid asteroid = _pool.Get();
            try
            {
                asteroid.transform.SetPositionAndRotation(position, Quaternion.identity);

                asteroid.Initialize(config, maxHealth);

                _enemyLifecycleService.Spawn(asteroid.PhysicsView, type, position, velocity, 0f);
            }
            catch
            {
                Return(asteroid);
                throw;
            }

            asteroid.gameObject.SetActive(true);

            return asteroid;
        }

        public void Return(Asteroid asteroid)
        {
            if (asteroid == null)
            {
                Debug.LogError("Cannot return a null asteroid", this);
                return;
            }

            if (asteroid.PhysicsView.IsBound && !_enemyLifecycleService.Despawn(asteroid.PhysicsView))
            {
                Debug.LogError("Bound asteroid physics view was not active", asteroid);
            }

            if (!_pool.Return(asteroid))
            {
                Debug.LogWarning("Asteroid is already in the pool", asteroid);
                return;
            }

            asteroid.Stop();
            asteroid.gameObject.SetActive(false);
        }

        public bool TryGetByEntity(EnemyEntity entity, out Asteroid asteroid)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            for (int i = 0; i < _pool.CreatedItems.Count; i++)
            {
                Asteroid candidate = _pool.CreatedItems[i];
                if (candidate == null || !candidate.PhysicsView.IsBound)
                {
                    continue;
                }

                if (ReferenceEquals(candidate.PhysicsView.Entity, entity))
                {
                    asteroid = candidate;
                    return true;
                }
            }

            asteroid = null;
            return false;
        }

        private Asteroid CreateAsteroid()
        {
            Asteroid asteroid = Instantiate(_asteroidPrefab, transform);

            asteroid.SetSortingOrder(_nextSortingOrder);
            _nextSortingOrder++;

            asteroid.Died += OnAsteroidDied;
            asteroid.gameObject.SetActive(false);

            return asteroid;
        }

        private void OnAsteroidDied(Asteroid asteroid, DeathSource deathSource)
        {
            EnemyType enemyType = asteroid.PhysicsView.Entity.Type;

            Return(asteroid);

            AsteroidDied?.Invoke(enemyType, deathSource);
        }

        private bool ValidateSerializedReferences()
        {
            if (_asteroidPrefab != null)
            {
                return true;
            }

            Debug.LogError("Asteroid prefab reference is missing", this);
            return false;
        }
    }
}
