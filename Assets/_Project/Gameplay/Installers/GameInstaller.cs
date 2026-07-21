using UnityEngine;
using Zenject;

namespace Game.Gameplay
{
    public sealed class GameInstaller : MonoInstaller
    {
        [SerializeField] private PlayerHealth _playerHealth;

        // ReSharper disable Unity.PerformanceAnalysis
        public override void InstallBindings()
        {
            BindSignals();
            BindPlayerHealth();
            BindSurvivalTimer();
            BindGameSession();
        }

        private void BindSignals()
        {
            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<PlayerDiedSignal>();
        }

        private void BindPlayerHealth()
        {
            if (_playerHealth == null)
            {
                throw new MissingReferenceException("PlayerHealth reference is missing in GameInstaller");
            }

            Container.Bind<PlayerHealth>().FromInstance(_playerHealth).AsSingle();
        }

        private void BindSurvivalTimer()
        {
            Container.BindInterfacesAndSelfTo<SurvivalTimer>().AsSingle().NonLazy();
        }

        private void BindGameSession()
        {
            Container.BindInterfacesTo<GameSession>().AsSingle().NonLazy();
        }
    }
}
