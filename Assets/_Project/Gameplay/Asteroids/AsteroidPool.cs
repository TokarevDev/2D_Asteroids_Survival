using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay
{
    public class AsteroidPool : MonoBehaviour
    {
        [SerializeField] private AsteroidMovement _asteroidPrefab;

        [SerializeField, Min(1)] private int _initialSize = 5;

        private readonly Queue<AsteroidMovement> _availableAsteroids = new();

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

        public AsteroidMovement Get(Vector2 position)
        {
            AsteroidMovement asteroid;

            if (_availableAsteroids.Count > 0)
            {
                asteroid = _availableAsteroids.Dequeue();
            }
            else
            {
                asteroid = CreateAsteroid();
            }

            asteroid.transform.SetPositionAndRotation(position, Quaternion.identity);

            asteroid.gameObject.SetActive(true);

            return asteroid;
        }

        public void Return(AsteroidMovement asteroid)
        {
            if (asteroid == null)
            {
                Debug.LogError("Cannot return a null asteroid", this);
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
                AsteroidMovement asteroid = CreateAsteroid();

                _availableAsteroids.Enqueue(asteroid);
            }
        }

        private AsteroidMovement CreateAsteroid()
        {
            AsteroidMovement asteroid = Instantiate(_asteroidPrefab, transform);

            asteroid.gameObject.SetActive(false);

            return asteroid;
        }
    }
}
