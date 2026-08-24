using Game.Core.Physics;
using Game.Gameplay.Player;
using Zenject;

namespace Game.Gameplay.Installers
{
    public sealed class PlayerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<PlayerPhysicsView>().FromComponentInHierarchy().AsSingle();

            Container.Bind<PlayerThrustView>().FromComponentInHierarchy().AsSingle();

            Container.BindInterfacesTo<PlayerThrustVisualController>().AsSingle().NonLazy();

            Container.Bind<PlayerInvulnerabilityView>().FromComponentInHierarchy().AsSingle();

            Container.BindInterfacesTo<PlayerInvulnerabilityVisualController>().AsSingle().NonLazy();

            Container.Bind<PlayerCollisionVfxPool>().FromComponentInHierarchy().AsSingle();

            Container.BindInterfacesTo<PlayerCollisionVfxService>().AsSingle().NonLazy();

            Container.BindInterfacesTo<PlayerCollisionDamageService>().AsSingle().NonLazy();

            Container.Bind<ElasticCollisionResolver2D>().AsSingle();

            Container.BindInterfacesTo<PlayerEnemyBounceService>().AsSingle().NonLazy();

            Container.Bind<PlayerHealth>().FromComponentInHierarchy().AsSingle();

            Container.BindInterfacesTo<PlayerDeathSignalService>().AsSingle().NonLazy();
        }
    }
}
