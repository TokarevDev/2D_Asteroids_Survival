using Game.Gameplay.Analytics;
using Game.Gameplay.Score;
using Game.Gameplay.Session;
using Game.Gameplay.Signals;
using Zenject;

namespace Game.Gameplay.Installers
{
    public sealed class GameSessionInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<PlayerDiedSignal>();
            Container.DeclareSignal<EnemyDiedSignal>();

            Container.BindInterfacesAndSelfTo<GamePauseService>().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<SurvivalTimer>().AsSingle().NonLazy();

            Container.Bind<ScoreCounter>().AsSingle().NonLazy();

            Container.BindInterfacesTo<EnemyRewardService>().AsSingle().NonLazy();

            Container.BindInterfacesTo<GameSession>().AsSingle().NonLazy();

            Container.BindInterfacesTo<GameAnalyticsReporter>().AsSingle().NonLazy();
        }
    }
}
