using UnityEngine;

namespace Game.Gameplay
{
    public class AsteroidSpawner : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private AsteroidMovement _asteroidPrefab;

        [SerializeField, Min(0f)] private float _spawnOffset = 2f;
        [SerializeField, Min(0f)] private float _targetOffset = 2f;
        [SerializeField, Min(0f)] private float _asteroidSpeed = 5f;

        private void Awake()
        {
            if (_camera == null)
            {
                Debug.LogError("Camera reference is missing", this);
                enabled = false;
                return;
            }

            if (_asteroidPrefab == null)
            {
                Debug.LogError("Asteroids prefab reference is missing", this);
                enabled = false;
            }
        }

        private void Start()
        {
            SpawnOne();
        }

        private void SpawnOne()
        {
            Vector2 spawnPosition = GetRandomSpawnPosition();
            Vector2 targetPosition = GetRandomTargetPosition();

            Debug.Log($"Spawn position: {spawnPosition}, Target: {targetPosition}", this);
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
