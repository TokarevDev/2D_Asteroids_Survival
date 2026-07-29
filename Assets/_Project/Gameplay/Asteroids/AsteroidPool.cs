using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class AsteroidPool : MonoBehaviour
    {
        public event Action<Asteroid, DeathSource> AsteroidDied;

        [SerializeField] private Asteroid _asteroidPrefab;

        [SerializeField, Min(1)] private int _initialSize = 5;
        private readonly HashSet<Asteroid> _availableAsteroidSet = new();
        private readonly Queue<Asteroid> _availableAsteroids = new();

        private readonly List<Asteroid> _createdAsteroids = new();

        private const int AsteroidSortingOrderBase = 100;

        private void Awake()
        {
            if (!ValidateSerializedReferences())
            {
                enabled = false;
                return;
            }

            Prewarm();
        }

        private void OnDestroy()
        {
            for (int i = 0; i < _createdAsteroids.Count; i++)
            {
                Asteroid asteroid = _createdAsteroids[i];
                if (asteroid == null)
                {
                    continue;
                }

                asteroid.Died -= OnAsteroidDied;
            }

            _createdAsteroids.Clear();
            _availableAsteroids.Clear();
            _availableAsteroidSet.Clear();
        }

        public Asteroid Get(Vector2 position)
        {
            Asteroid asteroid;

            if (_availableAsteroids.Count > 0)
            {
                asteroid = _availableAsteroids.Dequeue();
                _availableAsteroidSet.Remove(asteroid);
            }
            else
            {
                asteroid = CreateAsteroid();
            }

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

            if (!_availableAsteroidSet.Add(asteroid))
            {
                Debug.LogWarning("Asteroid is already in the pool", asteroid);
                return;
            }

            asteroid.Stop();
            asteroid.gameObject.SetActive(false);

            _availableAsteroids.Enqueue(asteroid);
        }

        private void Prewarm()
        {
            for (int i = 0; i < _initialSize; i++)
            {
                Asteroid asteroid = CreateAsteroid();

                Return(asteroid);
            }
        }

        private Asteroid CreateAsteroid()
        {
            Asteroid asteroid = Instantiate(_asteroidPrefab, transform);

            int sortingOrder = AsteroidSortingOrderBase + _createdAsteroids.Count;

            asteroid.SetSortingOrder(sortingOrder);

            asteroid.Died += OnAsteroidDied;

            _createdAsteroids.Add(asteroid);
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
