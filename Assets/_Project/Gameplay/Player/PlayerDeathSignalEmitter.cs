using UnityEngine;
using Zenject;

namespace Game.Gameplay
{
    public sealed class PlayerDeathSignalEmitter : MonoBehaviour
    {
        [SerializeField] private PlayerHealth _playerHealth;

        private SignalBus _signalBus;

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Awake()
        {
            if (_playerHealth == null)
            {
                Debug.LogError("PlayerHealth reference is missing", this);
                enabled = false;
            }
        }

        private void OnEnable()
        {
            _playerHealth.Died += OnPlayerDied;
        }

        private void OnDisable()
        {
            _playerHealth.Died -= OnPlayerDied;
        }

        private void OnPlayerDied()
        {
            _signalBus.Fire<PlayerDiedSignal>();
        }
    }
}
