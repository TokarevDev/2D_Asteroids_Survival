using UnityEngine;

namespace Game.Gameplay
{
    public class AsteroidSpawner : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private AsteroidPool _asteroidPool;

        [SerializeField, Min(0f)] private float _spawnOffset = 2f;
        [SerializeField, Min(0f)] private float _targetOffset = 2f;
        [SerializeField, Min(0f)] private float _asteroidSpeed = 5f;

        [SerializeField, Min(0.1f)] private float _spawnInterval = 1f;
        private float _timeUntilNextSpawn;

        private void Awake()
        {
            if (_camera == null)
            {
                Debug.LogError("Camera reference is missing", this);
                enabled = false;
                return;
            }

            if (_asteroidPool == null)
            {
                Debug.LogError("Asteroid pool reference is missing", this);
                enabled = false;
            }
        }

        private void Start()
        {
            SpawnOne();

            _timeUntilNextSpawn = _spawnInterval;
        }

        private void Update()
        {
            _timeUntilNextSpawn -= Time.deltaTime;

            if (_timeUntilNextSpawn > 0)
                return;

            SpawnOne();
            _timeUntilNextSpawn = _spawnInterval;
        }

        private void SpawnOne()
        {
            Vector2 spawnPosition = GetRandomSpawnPosition();
            Vector2 targetPosition = GetRandomTargetPosition();

            Vector2 direction = targetPosition - spawnPosition;

            AsteroidMovement asteroid = _asteroidPool.Get(spawnPosition);

            asteroid.Launch(direction, _asteroidSpeed);
        }

        private Vector2 GetRandomSpawnPosition()
        {
            Vector3 topLeft = _camera.ViewportToWorldPoint(new Vector3(0f, 1f, 0f));
            Vector3 topRight = _camera.ViewportToWorldPoint(new Vector3(1f, 1f, 0f));

            float randomX = Random.Range(topLeft.x, topRight.x);
            float spawnY = topLeft.y + _spawnOffset;

            return new Vector2(randomX, spawnY);
        }

        private Vector2 GetRandomTargetPosition()
        {
            Vector3 bottomLeft = _camera.ViewportToWorldPoint(new Vector3(0f, 0f, 0f));
            Vector3 bottomRight = _camera.ViewportToWorldPoint(new Vector3(1f, 0f, 0f));

            float randomX = Random.Range(bottomLeft.x, bottomRight.x);
            float targetY = bottomLeft.y - _targetOffset;

            return new Vector2(randomX, targetY);
        }
    }
}
