using System;
using System.Collections.Generic;
using Game.Core.Configuration;
using Game.Core.Enemies;
using Game.Core.Weapons;
using Game.Gameplay.Enemies;
using UnityEngine;

namespace Game.Gameplay.Weapons
{
    public sealed class LaserShotService
    {
        private readonly LaserTargetQuery _targetQuery;
        private readonly EnemyDestructionService _destructionService;
        private readonly List<EnemyEntity> _targets;

        private readonly float _laserLength;

        public LaserShotService(LaserTargetQuery targetQuery, EnemyDestructionService destructionService,
            IGameConfigProvider configProvider)
        {
            _targetQuery = targetQuery ?? throw new ArgumentNullException(nameof(targetQuery));

            _destructionService = destructionService ?? throw new ArgumentNullException(nameof(destructionService));

            if (configProvider == null)
            {
                throw new ArgumentNullException(nameof(configProvider));
            }

            _laserLength = configProvider.Player.LaserLength;
            _targets = new List<EnemyEntity>(configProvider.World.MaxEnemies);
        }

        public int Fire(Vector2 start, Vector2 direction)
        {
            if (direction.sqrMagnitude <= Mathf.Epsilon)
            {
                throw new ArgumentException("Laser direction cannot be zero", nameof(direction));
            }

            Vector2 end = start + direction.normalized * _laserLength;

            _targetQuery.CollectIntersecting(start, end, _targets);

            int destroyedCount = _targets.Count;

            for (int i = destroyedCount - 1; i >= 0; i--)
            {
                _destructionService.DestroyByPlayer(_targets[i]);
            }

            return destroyedCount;
        }
    }
}
