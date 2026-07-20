using Zenject;

namespace Game.UI
{
    public sealed class GameUIInstaller : MonoInstaller
    {
        // ReSharper disable Unity.PerformanceAnalysis
        public override void InstallBindings()
        {
            BindGameOverViewModel();
        }

        private void BindGameOverViewModel()
        {
            Container.BindInterfacesAndSelfTo<GameOverViewModel>().AsSingle().NonLazy();
        }
    }
}
