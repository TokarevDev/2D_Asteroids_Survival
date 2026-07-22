using Zenject;

namespace Game.UI
{
    public sealed class GameUIInstaller : MonoInstaller
    {
        // ReSharper disable Unity.PerformanceAnalysis
        public override void InstallBindings()
        {
            BindGameOverViewModel();
            BindHealthViewModel();
            BindTimerViewModel();
            BindScoreViewModel();
        }

        private void BindScoreViewModel()
        {
            Container.BindInterfacesAndSelfTo<ScoreViewModel>().AsSingle().NonLazy();
        }

        private void BindGameOverViewModel()
        {
            Container.BindInterfacesAndSelfTo<GameOverViewModel>().AsSingle().NonLazy();
        }

        private void BindHealthViewModel()
        {
            Container.BindInterfacesAndSelfTo<HealthViewModel>().AsSingle().NonLazy();
        }

        private void BindTimerViewModel()
        {
            Container.BindInterfacesAndSelfTo<TimerViewModel>().AsSingle().NonLazy();
        }
    }
}
