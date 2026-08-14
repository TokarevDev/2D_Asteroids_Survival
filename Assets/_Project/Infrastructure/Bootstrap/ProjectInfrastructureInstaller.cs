using Game.Core;
using Game.Core.Configuration;
using Game.Core.Input;
using Game.Infrastructure.Configuration;
using Game.Infrastructure.Controls;
using Game.Infrastructure.Performance;
using UnityEngine;
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
            BindPerformance();
            BindInput();
            BindAdvertisement();
        }

        private void BindPerformance()
        {
            Container.BindInterfacesTo<MobileFrameRateService>().AsSingle();
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
            Container.Bind<MobileInputBuffer>().AsSingle();

            Container.Bind<KeyboardMouseInputStrategy>().AsSingle();
            Container.Bind<MobileInputStrategy>().AsSingle();

            Container.Bind<IPlayerInputStrategy>().FromMethod(context =>
            {
                if (Application.isMobilePlatform)
                {
                    return context.Container.Resolve<MobileInputStrategy>();
                }

                return context.Container.Resolve<KeyboardMouseInputStrategy>();
            }).AsSingle();

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
