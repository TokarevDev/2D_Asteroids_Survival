using Game.Core.Enemies;
using Game.Gameplay.Asteroids;
using Game.Gameplay.Enemies;
using Game.Gameplay.Enemies.Ufo;
using Zenject;

namespace Game.Gameplay.Installers
{
    public sealed class EnemyLifecycleInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<EnemyRegistry>().AsSingle();
            Container.Bind<EnemyEntityFactory>().AsSingle();
            Container.Bind<EnemyEntityPool>().AsSingle();
            Container.Bind<EnemyLifecycleService>().AsSingle();

            Container.Bind<AsteroidPool>().FromComponentInHierarchy().AsSingle();

            Container.Bind<UfoPool>().FromComponentInHierarchy().AsSingle();

            Container.BindInterfacesTo<EnemyDeathSignalService>().AsSingle().NonLazy();

            Container.Bind<AsteroidFragmentSpawner>().AsSingle();
        }
    }
}
