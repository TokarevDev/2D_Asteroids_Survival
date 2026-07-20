using Zenject;

namespace Game.Gameplay
{
    public sealed class GameInstaller : MonoInstaller
    {
        // ReSharper disable Unity.PerformanceAnalysis
        public override void InstallBindings()
        {
            BindSignals();
            BindGameSession();
        }

        private void BindSignals()
        {
            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<PlayerDiedSignal>();
        }

        private void BindGameSession()
        {
            Container.BindInterfacesTo<GameSession>().AsSingle().NonLazy();
        }
    }
}
