using Game.Core;
using Game.Core.Configuration;
using Game.Core.Input;
using Game.Infrastructure.Configuration;
using Game.Infrastructure.Controls;
using Zenject;

namespace Game.Infrastructure
{
    public sealed class ProjectInfrastructureInstaller : MonoInstaller
    {
        // ReSharper disable Unity.PerformanceAnalysis
        public override void InstallBindings()
        {
            BindGameConfiguration();
            BindSceneLoader();
            BindApplicationQuitService();
            BindInput();
            BindAdvertisement();
        }

        private void BindGameConfiguration()
        {
            Container.BindInterfacesAndSelfTo<GameConfigProvider>().AsSingle();

            Container.Bind<GameConfigValidator>().AsSingle();

            Container.Bind<JsonConfigReader>().AsSingle();

            Container.Bind<IGameConfigLoader>().To<GameConfigLoader>().AsSingle();
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
            Container.Bind<IPlayerInputStrategy>().To<KeyboardMouseInputStrategy>().AsSingle();

            Container.Bind<IInputReader>().To<InputReader>()
                .AsSingle();
        }

        private void BindSceneLoader()
        {
            Container.Bind<ISceneLoader>()
                .To<SceneLoader>()
                .AsSingle();
        }
    }
}
