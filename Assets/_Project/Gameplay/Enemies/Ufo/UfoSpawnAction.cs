using System;
using Game.Core.Configuration;
using Game.Core.Enemies;
using Game.Gameplay.Enemies.Spawning;
using Game.Gameplay.World;
using UnityEngine;

namespace Game.Gameplay.Enemies.Ufo
{
    public sealed class UfoSpawnAction : IEnemySpawnAction
    {
        private readonly UfoPool _ufoPool;
        private readonly RandomWorldSpawnPointProvider _spawnPointProvider;
        private readonly float _speed;

        public UfoSpawnAction(UfoPool ufoPool, EnemyConfig enemyConfig,
            RandomWorldSpawnPointProvider spawnPointProvider)
        {
            _ufoPool = ufoPool ?? throw new ArgumentNullException(nameof(ufoPool));

            if (enemyConfig == null)
            {
                throw new ArgumentNullException(nameof(enemyConfig));
            }

            _spawnPointProvider = spawnPointProvider ?? throw new ArgumentNullException(nameof(spawnPointProvider));

            _speed = enemyConfig.GetParameters(EnemyType.Ufo).Speed;
        }

        public void Spawn()
        {
            Vector2 spawnPosition = _spawnPointProvider.GetSpawnPosition();
            Vector2 targetPosition = _spawnPointProvider.GetTargetPosition();
            Vector2 direction = (targetPosition - spawnPosition).normalized;
            Vector2 velocity = direction * _speed;

            _ufoPool.Get(spawnPosition, velocity);
        }
    }
}
