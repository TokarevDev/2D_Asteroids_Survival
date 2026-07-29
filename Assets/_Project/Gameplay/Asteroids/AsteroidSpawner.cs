using UnityEngine;
using Zenject;

namespace Game.Gameplay
{
    public sealed class AsteroidSpawner : MonoBehaviour
    {
        [SerializeField] private AsteroidPool _asteroidPool;
        [SerializeField] private AsteroidConfig[] _asteroidConfigs;

        [SerializeField, Min(0f)] private float _spawnOffset = 2f;
        [SerializeField, Min(0f)] private float _targetOffset = 2f;

        [SerializeField, Min(0.1f)] private float _spawnInterval = 1f;
        [SerializeField, Min(0.1f)] private float _minimumSpawnInterval = 0.5f;
        [SerializeField, Min(0f)] private float _spawnIntervalReductionPerMinute = 0.1f;

        private SurvivalTimer _survivalTimer;
        private float _timeUntilNextSpawn;

        private AsteroidConfigSelector _configSelector;
        private Camera _camera;

        [Inject]
        private void Construct(SurvivalTimer survivalTimer, CameraProvider cameraProvider)
        {
            _camera = cameraProvider.Camera;
            _survivalTimer = survivalTimer;
        }

        private void Awake()
        {
            if (!ValidateSerializedReferences() || !ValidateConfiguration())
            {
                enabled = false;
                return;
            }

            _configSelector = new AsteroidConfigSelector(_asteroidConfigs);
        }

        private void Start()
        {
            SpawnOne();

            _timeUntilNextSpawn = GetCurrentSpawnInterval();
        }

        private void Update()
        {
            _timeUntilNextSpawn -= Time.deltaTime;

            if (_timeUntilNextSpawn > 0)
            {
                return;
            }

            SpawnOne();
            _timeUntilNextSpawn = GetCurrentSpawnInterval();
        }

        private float GetCurrentSpawnInterval()
        {
            float elapsedMinutes = _survivalTimer.ElapsedSeconds / 60f;
            float intervalReduction = elapsedMinutes * _spawnIntervalReductionPerMinute;

            return Mathf.Max(_minimumSpawnInterval, _spawnInterval - intervalReduction);
        }

        private void SpawnOne()
        {
            Vector2 spawnPosition = GetRandomSpawnPosition();
            Vector2 targetPosition = GetRandomTargetPosition();

            Vector2 direction = targetPosition - spawnPosition;

            AsteroidConfig config = _configSelector.GetNextConfig();

            Asteroid asteroid = _asteroidPool.Get(spawnPosition);

            asteroid.Initialize(config);
            asteroid.Launch(direction);
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

        private bool ValidateConfiguration()
        {
            if (_minimumSpawnInterval <= _spawnInterval)
            {
                return true;
            }

            Debug.LogError(
                "Minimum spawn interval cannot exceed initial interval",
                this);

            return false;
        }

        private bool ValidateSerializedReferences()
        {
            bool isValid = true;

            if (_asteroidPool == null)
            {
                Debug.LogError("Asteroid pool reference is missing", this);
                isValid = false;
            }

            if (_asteroidConfigs == null || _asteroidConfigs.Length == 0)
            {
                Debug.LogError("Asteroid configs are missing", this);
                return false;
            }

            for (int i = 0; i < _asteroidConfigs.Length; i++)
            {
                if (_asteroidConfigs[i] != null)
                {
                    continue;
                }

                Debug.LogError(
                    $"Asteroid config at index {i} is missing",
                    this);

                isValid = false;
            }

            return isValid;
        }
    }
}
