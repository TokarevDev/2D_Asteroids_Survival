using Game.Core;
using Zenject;

namespace Game.Infrastructure
{
    public sealed class ProjectInfrastructureInstaller : MonoInstaller
    {
        // ReSharper disable Unity.PerformanceAnalysis
        public override void InstallBindings()
        {
            BindSceneLoader();
            BindApplicationQuitService();
            BindInput();
            BindAdvertisement();
        }

        private void BindApplicationQuitService()
        {
            Container.Bind<IApplicationQuitService>().To<ApplicationQuitService>().AsSingle();
        }

        private void BindAdvertisement()
        {
            Container.BindInterfacesTo<AdMobAdvertisementService>().AsSingle().NonLazy();
        }

        private void BindInput()
        {
            Container.BindInterfacesTo<InputReader>()
                .AsSingle()
                .NonLazy();
        }

        private void BindSceneLoader()
        {
            Container.Bind<ISceneLoader>()
                .To<SceneLoader>()
                .AsSingle();
        }
    }
}
