using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay
{
    public class AsteroidPool : MonoBehaviour
    {
        private const int AsteroidSortingOrderBase = 100;

        public event Action<int> AsteroidDestroyedByPlayer;

        [SerializeField] private Asteroid _asteroidPrefab;

        [SerializeField, Min(1)] private int _initialSize = 5;

        private readonly List<Asteroid> _createdAsteroids = new();
        private readonly Queue<Asteroid> _availableAsteroids = new();
        private readonly HashSet<Asteroid> _availableAsteroidSet = new();

        private void Awake()
        {
            if (_asteroidPrefab == null)
            {
                Debug.LogError("Asteroid prefab reference is missing", this);
                enabled = false;
                return;
            }

            Prewarm();
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

            asteroid.Died += Return;
            asteroid.DestroyedByPlayer += OnAsteroidDestroyedByPlayer;

            _createdAsteroids.Add(asteroid);
            asteroid.gameObject.SetActive(false);

            return asteroid;
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

                asteroid.Died -= Return;
                asteroid.DestroyedByPlayer -= OnAsteroidDestroyedByPlayer;
            }

            _createdAsteroids.Clear();
            _availableAsteroids.Clear();
            _availableAsteroidSet.Clear();
        }

        private void OnAsteroidDestroyedByPlayer(int scoreReward)
        {
            AsteroidDestroyedByPlayer?.Invoke(scoreReward);
        }
    }
}
