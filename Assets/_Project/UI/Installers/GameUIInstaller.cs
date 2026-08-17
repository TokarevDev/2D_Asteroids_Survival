using Game.UI.GameOver;
using Game.UI.HUD;
using Zenject;

namespace Game.UI.Installers
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
            BindPlayerTelemetryViewModel();
            BindLaserStatusViewModel();
        }

        private void BindLaserStatusViewModel()
        {
            Container.BindInterfacesAndSelfTo<LaserStatusViewModel>().AsSingle().NonLazy();
        }

        private void BindPlayerTelemetryViewModel()
        {
            Container.BindInterfacesAndSelfTo<PlayerTelemetryViewModel>().AsSingle().NonLazy();
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
