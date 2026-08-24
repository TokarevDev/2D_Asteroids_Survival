using Game.Core.Projectiles;
using Game.Gameplay.Projectiles;
using Zenject;

namespace Game.Gameplay.Installers
{
    public sealed class ProjectileLifecycleInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ProjectilePool>().FromComponentInHierarchy().AsSingle();

            Container.Bind<ProjectileEntityFactory>().AsSingle();
            Container.Bind<ProjectileRegistry>().AsSingle();
            Container.Bind<ProjectileEntityPool>().AsSingle();

            Container.Bind<ProjectileLifecycleService>().AsSingle().NonLazy();
        }
    }
}
