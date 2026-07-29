using UnityEngine;
using Zenject;

namespace Game.Gameplay
{
    public sealed class GameInstaller : MonoInstaller
    {
        // ReSharper disable Unity.PerformanceAnalysis
        public override void InstallBindings()
        {
            BindSignals();
            BindCameraProvider();
            BindPlayerHealth();
            BindPlayerDeathSignalService();
            BindAsteroidPool();
            BindSurvivalTimer();
            BindScoreCounter();
            BindAsteroidRewardService();
            BindGamePauseService();
            BindGameSession();
        }

        private void BindAsteroidRewardService()
        {
            Container.BindInterfacesTo<AsteroidRewardService>().AsSingle().NonLazy();
        }

        private void BindCameraProvider()
        {
            Container.Bind<Camera>().FromComponentInHierarchy().AsSingle();
            Container.Bind<CameraProvider>().AsSingle();
        }

        private void BindGamePauseService()
        {
            Container.BindInterfacesAndSelfTo<GamePauseService>().AsSingle().NonLazy();
        }

        private void BindPlayerDeathSignalService()
        {
            Container.BindInterfacesTo<PlayerDeathSignalService>().AsSingle().NonLazy();
        }

        private void BindScoreCounter()
        {
            Container.Bind<ScoreCounter>().AsSingle().NonLazy();
        }

        private void BindAsteroidPool()
        {
            Container.Bind<AsteroidPool>().FromComponentInHierarchy().AsSingle();
        }

        private void BindSignals()
        {
            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<PlayerDiedSignal>();
        }

        private void BindPlayerHealth()
        {
            Container.Bind<PlayerHealth>().FromComponentInHierarchy().AsSingle();
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
