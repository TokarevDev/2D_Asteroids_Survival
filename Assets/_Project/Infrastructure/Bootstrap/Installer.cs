using Game.Core;
using Zenject;

namespace Game.Infrastructure
{
    public class Installer : MonoInstaller
    {
        // ReSharper disable Unity.PerformanceAnalysis
        public override void InstallBindings()
        {
            BindSceneLoader();
            BindInput();
            BindAdvertisement();
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
