using System;
using Game.Core.Physics;
using Game.Core.Player;
using Game.Gameplay.Enemies;
using Game.Gameplay.Enemies.Ufo;
using Game.Gameplay.Loop;
using Game.Gameplay.Physics;
using Game.Gameplay.Player;
using Game.Gameplay.Projectiles;
using Zenject;

namespace Game.Gameplay.Installers
{
    public sealed class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindSharedServices();
            BindFixedLoopSystems();
            BindPlayerPhysicsController();
            BindProjectileImpactService();

            Container.BindInterfacesTo<GameplayFixedLoop>().AsSingle().NonLazy();
        }

        private void BindSharedServices()
        {
            Container.Bind<CustomPhysicsIntegrator2D>().AsSingle();
            Container.Bind<CustomPhysicsWorld2D>().AsSingle();
            Container.Bind<CircleCollisionDetector2D>().AsSingle();
            Container.Bind<PlayerInvulnerability>().AsSingle();
        }

        private void BindFixedLoopSystems()
        {
            Container.Bind<PlayerInvulnerabilityController>().AsSingle();
            Container.Bind<PlayerEnemyCollisionController>().AsSingle();
            Container.Bind<UfoPursuitController>().AsSingle();
            Container.Bind<ProjectileEnemyCollisionController>().AsSingle();
            Container.Bind<ProjectileLifetimeController>().AsSingle();
            Container.Bind<ProjectileWorldExitController>().AsSingle();
            Container.Bind<ProjectilePhysicsViewSynchronizer>().AsSingle();
            Container.Bind<EnemyWorldWrapController>().AsSingle();
            Container.Bind<EnemyPhysicsViewSynchronizer>().AsSingle();
            Container.Bind<PlayerWorldWrapController>().AsSingle();
            Container.Bind<PlayerPhysicsViewSynchronizer>().AsSingle();
            Container.Bind<CustomPhysicsFixedTickRunner>().AsSingle();
        }

        private void BindPlayerPhysicsController()
        {
            Container.Bind(
                    typeof(PlayerPhysicsController), typeof(IInitializable), typeof(IDisposable))
                .To<PlayerPhysicsController>()
                .AsSingle()
                .NonLazy();
        }

        private void BindProjectileImpactService()
        {
            Container.BindInterfacesTo<ProjectileImpactService>()
                .AsSingle()
                .NonLazy();
        }
    }
}
