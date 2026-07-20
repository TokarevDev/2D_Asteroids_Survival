using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay
{
    public class AsteroidPool : MonoBehaviour
    {
        [SerializeField] private Asteroid _asteroidPrefab;

        [SerializeField, Min(1)] private int _initialSize = 5;

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

            asteroid.Died += Return;
            asteroid.gameObject.SetActive(false);

            return asteroid;
        }
    }
}
