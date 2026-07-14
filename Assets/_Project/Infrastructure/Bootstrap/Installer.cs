using Game.Core;
using Zenject;

namespace Game.Infrastructure
{
    public class Installer : MonoInstaller
    {
        // ReSharper disable Unity.PerformanceAnalysis
        public override void InstallBindings()
        {
            BindInput();
            BindSceneLoader();
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
