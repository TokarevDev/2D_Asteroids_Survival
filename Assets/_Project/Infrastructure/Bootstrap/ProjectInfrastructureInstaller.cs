using System;
using Game.Core.Analytics;
using Game.Core.Application;
using Game.Core.Configuration;
using Game.Core.Input;
using Game.Core.Navigation;
using Game.Core.Scenes;
using Game.Infrastructure.Advertising;
using Game.Infrastructure.Analytics;
using Game.Infrastructure.Application;
using Game.Infrastructure.Configuration;
using Game.Infrastructure.Controls;
using Game.Infrastructure.Performance;
using UnityEngine;
using Zenject;
using UnityApplication = UnityEngine.Application;

namespace Game.Infrastructure.Bootstrap
{
    public sealed class ProjectInfrastructureInstaller : MonoInstaller
    {
        [SerializeField] private AdMobConfiguration _adMobConfiguration;

        // ReSharper disable Unity.PerformanceAnalysis
        public override void InstallBindings()
        {
            BindGameConfiguration();
            BindAnalytics();
            BindSceneLoader();
            BindNavigation();
            BindApplicationQuitService();
            BindPerformance();
            BindInput();
            BindAdvertisement();
        }

        private void BindAnalytics()
        {
            Container.Bind<FirebaseInitializer>().AsSingle();

            Container.Bind<IAnalyticsService>().To<FirebaseAnalyticsService>().AsSingle();
        }

        private void BindNavigation()
        {
            Container.Bind<GameNavigationFacade>().AsSingle();
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
            if (_adMobConfiguration == null)
            {
                throw new InvalidOperationException("AdMob configuration reference is missing");
            }

            Container.BindInstance(_adMobConfiguration);

            Container.BindInterfacesAndSelfTo<AdMobInitializer>().AsSingle().NonLazy();

            Container.BindInterfacesTo<BannerAdvertisementService>().AsSingle().NonLazy();
        }

        private void BindInput()
        {
            Container.Bind<MobileInputBuffer>().AsSingle();

            Container.Bind<KeyboardMouseInputStrategy>().AsSingle();
            Container.Bind<MobileInputStrategy>().AsSingle();

            Container.Bind<IPlayerInputStrategy>().FromMethod(context =>
            {
                if (UnityApplication.isMobilePlatform)
                {
                    return context.Container.Resolve<MobileInputStrategy>();
                }

                return context.Container.Resolve<KeyboardMouseInputStrategy>();
            }).AsSingle();
        }

        private void BindSceneLoader()
        {
            Container.Bind<ISceneLoader>()
                .To<SceneLoader>()
                .AsSingle();
        }
    }
}
