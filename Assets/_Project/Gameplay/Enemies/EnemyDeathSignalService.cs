using System;
using Game.Core.Enemies;
using Game.Gameplay.Combat;
using Game.Gameplay.Signals;
using Zenject;

namespace Game.Gameplay.Enemies
{
    public sealed class EnemyDeathSignalService : IInitializable, IDisposable
    {
        private readonly EnemyDeathEventSource _deathEventSource;
        private readonly SignalBus _signalBus;

        public EnemyDeathSignalService(EnemyDeathEventSource deathEventSource, SignalBus signalBus)
        {
            _deathEventSource = deathEventSource ?? throw new ArgumentNullException(nameof(deathEventSource));
            _signalBus = signalBus ?? throw new ArgumentNullException(nameof(signalBus));
        }

        public void Initialize()
        {
            _deathEventSource.EnemyDied += OnEnemyDied;
        }

        public void Dispose()
        {
            _deathEventSource.EnemyDied -= OnEnemyDied;
        }

        private void OnEnemyDied(EnemyType enemyType, DeathSource deathSource)
        {
            _signalBus.Fire(new EnemyDiedSignal(enemyType, deathSource));
        }
    }
}
