using Game.Core.Configuration;
using Game.Core.Enemies;
using Game.Core.Physics;
using Game.Core.Projectiles;
using Game.Core.World;
using Game.Gameplay.Enemies;
using Game.Gameplay.Enemies.Ufo;
using Game.Gameplay.Physics;
using Game.Gameplay.Player;
using Game.Gameplay.Projectiles;
using Game.Gameplay.World;
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
            BindProjectileCollisionDetection();
            BindProjectilePool();
            BindProjectileEntities();
            BindEnemyRegistry();
            BindEnemyEntityFactory();
            BindEnemyEntityPool();
            BindEnemyLifecycleService();
            BindEnemyPhysicsViewSynchronizer();
            BindToroidalWorld();
            BindRandomWorldSpawnPointProvider();
            BindEnemyWorldWrapController();
            BindPlayerPhysicsView();
            BindPlayerPhysicsController();
            BindPlayerWorldWrapController();
            BindPlayerPhysicsViewSynchronizer();
            BindSignals();
            BindCameraProvider();
            BindPlayerHealth();
            BindPlayerDeathSignalService();
            BindAsteroidPool();
            BindUfoPool();
            BindUfoPursuitController();
            BindAsteroidFragmentSpawner();
            BindSurvivalTimer();
            BindScoreCounter();
            BindAsteroidRewardService();
            BindGamePauseService();
            BindGameSession();
        }

        private void BindUfoPursuitController()
        {
            Container.BindInterfacesTo<UfoPursuitController>().AsSingle().NonLazy();

            Container.BindFixedTickableExecutionOrder<UfoPursuitController>(-50);
        }

        private void BindUfoPool()
        {
            Container.Bind<UfoPool>().FromComponentInHierarchy().AsSingle();
        }

        private void BindAsteroidFragmentSpawner()
        {
            Container.Bind<AsteroidFragmentSpawner>().AsSingle();
        }

        private void BindRandomWorldSpawnPointProvider()
        {
            Container.Bind<RandomWorldSpawnPointProvider>().AsSingle();
        }

        private void BindProjectileCollisionDetection()
        {
            Container.Bind<CircleCollisionDetector2D>().AsSingle();

            Container.BindInterfacesAndSelfTo<ProjectileEnemyCollisionController>().AsSingle().NonLazy();

            Container.BindInterfacesTo<ProjectileImpactService>().AsSingle().NonLazy();

            Container.BindFixedTickableExecutionOrder<ProjectileEnemyCollisionController>(75);
        }

        private void BindProjectilePool()
        {
            Container.Bind<ProjectilePool>().FromComponentInHierarchy().AsSingle();
        }

        private void BindProjectileEntities()
        {
            Container.Bind<ProjectileEntityFactory>().AsSingle();

            Container.Bind<ProjectileRegistry>().AsSingle();

            Container.Bind<ProjectileEntityPool>().AsSingle();

            Container.Bind<ProjectileLifecycleService>().AsSingle().NonLazy();

            Container.BindInterfacesTo<ProjectileLifetimeController>().AsSingle().NonLazy();

            Container.BindFixedTickableExecutionOrder<ProjectileLifetimeController>(10);

            Container.BindInterfacesTo<ProjectileWorldExitController>().AsSingle().NonLazy();

            Container.BindFixedTickableExecutionOrder<ProjectileWorldExitController>(25);

            Container.BindInterfacesAndSelfTo<ProjectilePhysicsViewSynchronizer>().AsSingle().NonLazy();

            Container.BindFixedTickableExecutionOrder<ProjectilePhysicsViewSynchronizer>(100);
        }

        private void BindEnemyWorldWrapController()
        {
            Container.BindInterfacesTo<EnemyWorldWrapController>().AsSingle().NonLazy();

            Container.BindFixedTickableExecutionOrder<EnemyWorldWrapController>(50);
        }

        private void BindEnemyPhysicsViewSynchronizer()
        {
            Container.BindInterfacesAndSelfTo<EnemyPhysicsViewSynchronizer>().AsSingle().NonLazy();

            Container.BindFixedTickableExecutionOrder<EnemyPhysicsViewSynchronizer>(100);
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
            Container.BindFixedTickableExecutionOrder<CustomPhysicsFixedTickRunner>(0);
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
