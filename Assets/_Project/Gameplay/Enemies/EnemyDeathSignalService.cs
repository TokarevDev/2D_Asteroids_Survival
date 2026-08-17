using System;
using Game.Core.Enemies;
using Game.Gameplay.Combat;
using Game.Gameplay.Enemies.Ufo;
using Game.Gameplay.Signals;
using Zenject;

namespace Game.Gameplay.Enemies
{
    public sealed class EnemyDeathSignalService : IInitializable, IDisposable
    {
        private readonly AsteroidPool _asteroidPool;

        private readonly UfoPool _ufoPool;
        private readonly SignalBus _signalBus;

        public EnemyDeathSignalService(AsteroidPool asteroidPool, UfoPool ufoPool, SignalBus signalBus)
        {
            _asteroidPool = asteroidPool ?? throw new ArgumentNullException(nameof(asteroidPool));

            _ufoPool = ufoPool ?? throw new ArgumentNullException(nameof(ufoPool));

            _signalBus = signalBus ?? throw new ArgumentNullException(nameof(signalBus));
        }

        public void Initialize()
        {
            _asteroidPool.AsteroidDied += OnEnemyDied;
            _ufoPool.UfoDied += OnEnemyDied;
        }

        public void Dispose()
        {
            _asteroidPool.AsteroidDied -= OnEnemyDied;
            _ufoPool.UfoDied -= OnEnemyDied;
        }

        private void OnEnemyDied(EnemyType enemyType, DeathSource deathSource)
        {
            _signalBus.Fire(new EnemyDiedSignal(enemyType, deathSource));
        }
    }
}
