using System;
using System.Collections.Generic;
using Game.Core.Enemies;
using Game.Core.Physics;
using UnityEngine;

namespace Game.Core.Weapons
{
    public sealed class LaserTargetQuery
    {
        private readonly EnemyRegistry _enemyRegistry;
        private readonly SegmentCircleIntersectionDetector2D _intersectionDetector;

        public LaserTargetQuery(EnemyRegistry enemyRegistry, SegmentCircleIntersectionDetector2D intersectionDetector)
        {
            _enemyRegistry = enemyRegistry ?? throw new ArgumentNullException(nameof(enemyRegistry));

            _intersectionDetector =
                intersectionDetector ?? throw new ArgumentNullException(nameof(intersectionDetector));
        }

        public void CollectIntersecting(Vector2 segmentStart, Vector2 segmentEnd, List<EnemyEntity> results)
        {
            if (results == null)
            {
                throw new ArgumentNullException(nameof(results));
            }

            results.Clear();

            IReadOnlyList<EnemyEntity> enemies = _enemyRegistry.Enemies;

            for (int i = 0; i < enemies.Count; i++)
            {
                EnemyEntity enemy = enemies[i];

                if (_intersectionDetector.Intersects(segmentStart, segmentEnd, enemy.PhysicsBody.Position,
                        enemy.PhysicsBody.CollisionRadius))
                {
                    results.Add(enemy);
                }
            }
        }
    }
}
