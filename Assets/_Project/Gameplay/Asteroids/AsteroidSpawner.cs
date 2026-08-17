using System;
using Game.Core.Configuration;
using Game.Core.Enemies;
using Game.Gameplay.Session;
using Game.Gameplay.World;
using UnityEngine;
using Zenject;

namespace Game.Gameplay.Asteroids
{
    public sealed class AsteroidSpawner : MonoBehaviour
    {
        [SerializeField] private AsteroidPool _asteroidPool;
        [SerializeField] private AsteroidConfig[] _asteroidConfigs;

        private SurvivalTimer _survivalTimer;
        private float _timeUntilNextSpawn;

        private IGameConfigProvider _configProvider;
        private EnemyRegistry _enemyRegistry;

        private AsteroidConfigSelector _configSelector;
        private RandomWorldSpawnPointProvider _spawnPointProvider;

        [Inject]
        private void Construct(EnemyRegistry enemyRegistry, IGameConfigProvider configProvider,
            SurvivalTimer survivalTimer,
            RandomWorldSpawnPointProvider spawnPointProvider)
        {
            _enemyRegistry = enemyRegistry ?? throw new ArgumentNullException(nameof(enemyRegistry));
            _configProvider = configProvider ?? throw new ArgumentNullException(nameof(configProvider));
            _survivalTimer = survivalTimer ?? throw new ArgumentNullException(nameof(survivalTimer));
            _spawnPointProvider = spawnPointProvider ?? throw new ArgumentNullException(nameof(spawnPointProvider));
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
            float intervalReduction = elapsedMinutes * _configProvider.Enemy.AsteroidSpawnIntervalReductionPerMinute;
            float initialInterval = _configProvider.Enemy.AsteroidSpawnIntervalSeconds;
            float minimumInterval = _configProvider.Enemy.MinimumAsteroidSpawnIntervalSeconds;

            return Mathf.Max(minimumInterval, initialInterval - intervalReduction);
        }

        private void SpawnOne()
        {
            if (_enemyRegistry.Count >= _configProvider.World.MaxEnemies)
            {
                return;
            }

            Vector2 spawnPosition = _spawnPointProvider.GetSpawnPosition();
            Vector2 targetPosition = _spawnPointProvider.GetTargetPosition();

            Vector2 direction = targetPosition - spawnPosition;

            EnemyParameters parameters = _configProvider.Enemy.GetParameters(EnemyType.LargeAsteroid);

            Vector2 velocity = direction.normalized * parameters.Speed;

            AsteroidConfig config = _configSelector.GetNextConfig();

            _asteroidPool.Get(config, EnemyType.LargeAsteroid, spawnPosition, velocity, parameters.MaxHealth);
        }

        private bool ValidateConfiguration()
        {
            float initialInterval = _configProvider.Enemy.AsteroidSpawnIntervalSeconds;
            float minimumInterval = _configProvider.Enemy.MinimumAsteroidSpawnIntervalSeconds;
            if (minimumInterval <= initialInterval)
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
