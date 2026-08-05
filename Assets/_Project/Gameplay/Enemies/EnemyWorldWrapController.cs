using System;
using System.Collections.Generic;
using Game.Core.Enemies;
using Game.Core.Physics;
using Game.Core.World;
using Zenject;

namespace Game.Gameplay.Enemies
{
    public sealed class EnemyWorldWrapController : IFixedTickable
    {
        private readonly EnemyRegistry _registry;
        private readonly ToroidalWorld2D _world;

        public EnemyWorldWrapController(EnemyRegistry registry, ToroidalWorld2D world)
        {
            _registry = registry ?? throw new ArgumentNullException(nameof(registry));
            _world = world ?? throw new ArgumentNullException(nameof(world));
        }

        public void FixedTick()
        {
            IReadOnlyList<EnemyEntity> enemies = _registry.Enemies;

            for (int i = 0; i < enemies.Count; i++)
            {
                EnemyEntity enemy = enemies[i];
                CustomPhysicsBody2D body = enemy.PhysicsBody;
                if (!enemy.HasEnteredWorld)
                {
                    if (!_world.Contains(body.Position))
                    {
                        continue;
                    }

                    enemy.MarkAsEnteredWorld();
                }

                _world.Wrap(body);
            }
        }
    }
}
