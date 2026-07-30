using System;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class AsteroidPool : MonoBehaviour
    {
        public event Action<Asteroid, DeathSource> AsteroidDied;

        private const int AsteroidSortingOrderBase = 100;

        [SerializeField] private Asteroid _asteroidPrefab;
        [SerializeField, Min(1)] private int _initialSize = 5;

        private ObjectPool<Asteroid> _pool;
        private int _nextSortingOrder = AsteroidSortingOrderBase;

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

        public Asteroid Get(Vector2 position)
        {
            Asteroid asteroid = _pool.Get();

            asteroid.transform.SetPositionAndRotation(position, Quaternion.identity);

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
