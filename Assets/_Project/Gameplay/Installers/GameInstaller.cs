using Game.Core.Configuration;
using Game.Core.Enemies;
using Game.Core.Physics;
using Game.Core.World;
using Game.Gameplay.Enemies;
using Game.Gameplay.Physics;
using Game.Gameplay.Player;
using UnityEngine;
using Zenject;

namespace Game.Gameplay.Installers
{
    public sealed class GameInstaller : MonoInstaller
    {
        // ReSharper disable Unity.PerformanceAnalysis
        public override void InstallBindings()
        {
            BindCustomPhysics();
            BindEnemyRegistry();
            BindEnemyEntityFactory();
            BindEnemyEntityPool();
            BindEnemyLifecycleService();
            BindToroidalWorld();
            BindPlayerPhysicsView();
            BindPlayerPhysicsController();
            BindPlayerWorldWrapController();
            BindPlayerPhysicsViewSynchronizer();
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

        private void BindEnemyLifecycleService()
        {
            Container.Bind<EnemyLifecycleService>().AsSingle();
        }

        private void BindEnemyEntityPool()
        {
            Container.Bind<EnemyEntityPool>().AsSingle();
        }

        private void BindEnemyEntityFactory()
        {
            Container.Bind<EnemyEntityFactory>().AsSingle();
        }

        private void BindEnemyRegistry()
        {
            Container.Bind<EnemyRegistry>().AsSingle();
        }

        private void BindPlayerWorldWrapController()
        {
            Container.BindInterfacesTo<PlayerWorldWrapController>().AsSingle().NonLazy();

            Container.BindFixedTickableExecutionOrder<PlayerWorldWrapController>(50);
        }

        private void BindToroidalWorld()
        {
            Container.Bind<ToroidalWorld2D>().FromMethod(context =>
            {
                WorldConfig config = context.Container.Resolve<IGameConfigProvider>().World;

                return new ToroidalWorld2D(config.Width, config.Height);
            }).AsSingle();
        }

        private void BindPlayerPhysicsViewSynchronizer()
        {
            Container.BindInterfacesTo<PlayerPhysicsViewSynchronizer>().AsSingle().NonLazy();

            Container.BindFixedTickableExecutionOrder<PlayerPhysicsViewSynchronizer>(100);
        }

        private void BindPlayerPhysicsController()
        {
            Container.BindInterfacesAndSelfTo<PlayerPhysicsController>().AsSingle().NonLazy();

            Container.BindFixedTickableExecutionOrder<PlayerPhysicsController>(-100);
        }

        private void BindPlayerPhysicsView()
        {
            Container.Bind<PlayerPhysicsView>().FromComponentInHierarchy().AsSingle();
        }

        private void BindCustomPhysics()
        {
            Container.Bind<CustomPhysicsIntegrator2D>().AsSingle();
            Container.Bind<CustomPhysicsWorld2D>().AsSingle();

            Container.BindInterfacesTo<CustomPhysicsFixedTickRunner>().AsSingle().NonLazy();
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
