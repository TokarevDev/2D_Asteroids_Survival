using System;
using Game.Core.Configuration;
using Game.Core.World;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Gameplay.World
{
    public sealed class RandomWorldSpawnPointProvider
    {
        private readonly ToroidalWorld2D _world;
        private readonly float _outsideOffset;

        private enum WorldEdge
        {
            Left,
            Right,
            Bottom,
            Top,
            Count
        }

        public RandomWorldSpawnPointProvider(IGameConfigProvider configProvider, ToroidalWorld2D world)
        {
            if (configProvider == null)
            {
                throw new ArgumentNullException(nameof(configProvider));
            }

            _world = world ?? throw new ArgumentNullException(nameof(world));
            _outsideOffset = configProvider.World.SpawnOutsideOffset;
        }

        public Vector2 GetSpawnPosition()
        {
            float halfWidth = _world.HalfWidth;
            float halfHeight = _world.HalfHeight;

            WorldEdge edge = (WorldEdge)Random.Range(0, (int)WorldEdge.Count);

            switch (edge)
            {
                case WorldEdge.Left:
                    return new Vector2(-halfWidth - _outsideOffset, Random.Range(-halfHeight, halfHeight));

                case WorldEdge.Right:
                    return new Vector2(halfWidth + _outsideOffset, Random.Range(-halfHeight, halfHeight));

                case WorldEdge.Bottom:
                    return new Vector2(Random.Range(-halfWidth, halfWidth), -halfHeight - _outsideOffset);

                case WorldEdge.Top:
                    return new Vector2(Random.Range(-halfWidth, halfWidth), halfHeight + _outsideOffset);

                default:
                    throw new ArgumentOutOfRangeException(nameof(edge), edge, "Unsupported world edge");
            }
        }

        public Vector2 GetTargetPosition()
        {
            return new Vector2(
                Random.Range(-_world.HalfWidth, _world.HalfWidth), Random.Range(-_world.HalfHeight, _world.HalfHeight));
        }
    }
}
