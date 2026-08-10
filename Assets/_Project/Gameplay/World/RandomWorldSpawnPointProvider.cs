using System;
using Game.Core.Configuration;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Gameplay.World
{
    public sealed class RandomWorldSpawnPointProvider
    {
        private readonly float _halfWidth;
        private readonly float _halfHeight;
        private readonly float _outsideOffset;

        private enum WorldEdge
        {
            Left,
            Right,
            Bottom,
            Top,
            Count
        }

        public RandomWorldSpawnPointProvider(IGameConfigProvider configProvider)
        {
            if (configProvider == null)
            {
                throw new ArgumentNullException(nameof(configProvider));
            }

            WorldConfig config = configProvider.World;

            _halfWidth = config.Width * 0.5f;
            _halfHeight = config.Height * 0.5f;
            _outsideOffset = config.SpawnOutsideOffset;
        }

        public Vector2 GetSpawnPosition()
        {
            WorldEdge edge = (WorldEdge)Random.Range(0, (int)WorldEdge.Count);

            switch (edge)
            {
                case WorldEdge.Left:
                    return new Vector2(-_halfWidth - _outsideOffset, Random.Range(-_halfHeight, _halfHeight));

                case WorldEdge.Right:
                    return new Vector2(_halfWidth + _outsideOffset, Random.Range(-_halfHeight, _halfHeight));

                case WorldEdge.Bottom:
                    return new Vector2(Random.Range(-_halfWidth, _halfWidth), -_halfHeight - _outsideOffset);

                case WorldEdge.Top:
                    return new Vector2(Random.Range(-_halfWidth, _halfWidth), _halfHeight + _outsideOffset);

                default:
                    throw new ArgumentOutOfRangeException(nameof(edge), edge, "Unsupported world edge");
            }
        }

        public Vector2 GetTargetPosition()
        {
            return new Vector2(
                Random.Range(-_halfWidth, _halfWidth), Random.Range(-_halfHeight, _halfHeight));
        }
    }
}
