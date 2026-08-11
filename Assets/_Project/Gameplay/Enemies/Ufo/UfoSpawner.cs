using System;
using Game.Core.Configuration;
using Game.Core.Enemies;
using Game.Gameplay.World;
using UnityEngine;
using Zenject;

namespace Game.Gameplay.Enemies.Ufo
{
    public sealed class UfoSpawner : MonoBehaviour
    {
        [SerializeField] private UfoPool _ufoPool;

        private EnemyRegistry _enemyRegistry;
        private IGameConfigProvider _configProvider;
        private RandomWorldSpawnPointProvider _spawnPointProvider;

        private float _timeUntilNextSpawn;

        [Inject]
        private void Construct(EnemyRegistry enemyRegistry, IGameConfigProvider configProvider,
            RandomWorldSpawnPointProvider spawnPointProvider)
        {
            _enemyRegistry = enemyRegistry ?? throw new ArgumentNullException(nameof(enemyRegistry));

            _configProvider = configProvider ?? throw new ArgumentNullException(nameof(configProvider));

            _spawnPointProvider = spawnPointProvider ?? throw new ArgumentNullException(nameof(spawnPointProvider));
        }

        private void Awake()
        {
            if (_ufoPool != null)
            {
                return;
            }

            Debug.LogError("UFO pool reference is missing", this);
            enabled = false;
        }

        private void Start()
        {
            SpawnOne();
            ResetSpawnTimer();
        }

        private void Update()
        {
            _timeUntilNextSpawn -= Time.deltaTime;

            if (_timeUntilNextSpawn > 0f)
            {
                return;
            }

            SpawnOne();
            ResetSpawnTimer();
        }

        private void SpawnOne()
        {
            if (_enemyRegistry.Count >= _configProvider.World.MaxEnemies)
            {
                return;
            }

            Vector2 spawnPosition = _spawnPointProvider.GetSpawnPosition();

            Vector2 targetPosition = _spawnPointProvider.GetTargetPosition();

            Vector2 direction = (targetPosition - spawnPosition).normalized;

            EnemyParameters parameters = _configProvider.Enemy.GetParameters(EnemyType.Ufo);

            Vector2 velocity = direction * parameters.Speed;

            _ufoPool.Get(spawnPosition, velocity);
        }

        private void ResetSpawnTimer()
        {
            _timeUntilNextSpawn = _configProvider.Enemy.UfoSpawnIntervalSeconds;
        }
    }
}
