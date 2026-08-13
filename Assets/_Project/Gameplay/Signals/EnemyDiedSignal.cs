using Game.Core.Enemies;

namespace Game.Gameplay.Signals
{
    public sealed class EnemyDiedSignal
    {
        public EnemyType EnemyType { get; }
        public DeathSource DeathSource { get; }

        public EnemyDiedSignal(EnemyType enemyType, DeathSource deathSource)
        {
            EnemyType = enemyType;
            DeathSource = deathSource;
        }
    }
}
