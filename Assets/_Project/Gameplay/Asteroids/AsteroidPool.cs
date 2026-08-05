using System;
using Game.Core.Enemies;
using Game.Gameplay.Enemies;
using UnityEngine;
using Zenject;

namespace Game.Gameplay
{
    public sealed class AsteroidPool : MonoBehaviour
    {
        public event Action<Asteroid, DeathSource> AsteroidDied;

        private const int AsteroidSortingOrderBase = 100;

        [SerializeField] private Asteroid _asteroidPrefab;
        [SerializeField, Min(1)] private int _initialSize = 5;

        private EnemyLifecycleService _enemyLifecycleService;

        private ObjectPool<Asteroid> _pool;
        private int _nextSortingOrder = AsteroidSortingOrderBase;

        [Inject]
        private void Construct(EnemyLifecycleService enemyLifecycleService)
        {
            _enemyLifecycleService =
                enemyLifecycleService ?? throw new ArgumentNullException(nameof(enemyLifecycleService));
        }

        private void Awake()
        {
            if (!ValidateSerializedReferences())
            {
                enabled = false;
                return;
            }

            _pool = new ObjectPool<Asteroid>(CreateAsteroid, _initialSize);
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

        public Asteroid Get(AsteroidConfig config, EnemyType type, Vector2 position, Vector2 velocity)
        {
            Asteroid asteroid = _pool.Get();
            try
            {
                asteroid.transform.SetPositionAndRotation(position, Quaternion.identity);

                asteroid.Initialize(config);

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
            Return(asteroid);
            AsteroidDied?.Invoke(asteroid, deathSource);
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
