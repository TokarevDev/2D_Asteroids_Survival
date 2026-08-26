using System;
using Game.Core.Enemies;
using Game.Gameplay.Combat;

namespace Game.Gameplay.Enemies
{
    public sealed class EnemyDeathEventSource
    {
        public event Action<EnemyType, DeathSource> EnemyDied;

        public void Publish(EnemyType enemyType, DeathSource deathSource)
        {
            EnemyDied?.Invoke(enemyType, deathSource);
        }
    }
}
