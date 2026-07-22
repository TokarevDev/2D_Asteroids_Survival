using UnityEngine;
using Zenject;

namespace Game.Gameplay
{
    public sealed class GameInstaller : MonoInstaller
    {
        [SerializeField] private PlayerHealth _playerHealth;
        [SerializeField] private AsteroidPool _asteroidPool;

        // ReSharper disable Unity.PerformanceAnalysis
        public override void InstallBindings()
        {
            BindSignals();
            BindPlayerHealth();
            BindAsteroidPool();
            BindSurvivalTimer();
            BindScoreCounter();
            BindGameSession();
        }

        private void BindScoreCounter()
        {
            Container.BindInterfacesAndSelfTo<ScoreCounter>().AsSingle().NonLazy();
        }

        private void BindAsteroidPool()
        {
            if (_asteroidPool == null)
            {
                throw new MissingReferenceException("AsteroidPool reference is missing in GameInstaller");
            }

            Container.Bind<AsteroidPool>().FromInstance(_asteroidPool).AsSingle();
        }

        private void BindSignals()
        {
            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<PlayerDiedSignal>();
        }

        private void BindPlayerHealth()
        {
            if (_playerHealth == null)
            {
                throw new MissingReferenceException("PlayerHealth reference is missing in GameInstaller");
            }

            Container.Bind<PlayerHealth>().FromInstance(_playerHealth).AsSingle();
        }

        private void BindSurvivalTimer()
        {
            Container.BindInterfacesAndSelfTo<SurvivalTimer>().AsSingle().NonLazy();
        }

        private void BindGameSession()
        {
            Container.BindInterfacesTo<GameSession>().AsSingle().NonLazy();
        }
    }
}
