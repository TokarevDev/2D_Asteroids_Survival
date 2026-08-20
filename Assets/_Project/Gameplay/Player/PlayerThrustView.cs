using UnityEngine;

namespace Game.Gameplay.Player
{
    public enum PlayerThrusterLevel
    {
        Off,
        Low,
        Half,
        Max
    }

    [DisallowMultipleComponent]
    public sealed class PlayerThrustView : MonoBehaviour
    {
        private const float LowEmissionRate = 8f;
        private const float HalfEmissionRate = 16f;
        private const float MaxEmissionRate = 28f;

        [SerializeField] private ParticleSystem _leftParticles;
        [SerializeField] private ParticleSystem _rightParticles;

        private PlayerThrusterLevel _currentLevel;

        private void Awake()
        {
            if (_leftParticles == null || _rightParticles == null)
            {
                Debug.LogError("Player thruster ParticleSystem reference is missing", this);
                enabled = false;
                return;
            }

            _leftParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            _rightParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            _currentLevel = PlayerThrusterLevel.Off;
        }

        private void OnDisable()
        {
            if (_leftParticles == null || _rightParticles == null)
            {
                return;
            }

            _leftParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            _rightParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            _currentLevel = PlayerThrusterLevel.Off;
        }

        public void SetLevel(PlayerThrusterLevel level)
        {
            if (_leftParticles == null || _rightParticles == null || level == _currentLevel)
            {
                return;
            }

            _currentLevel = level;

            if (level == PlayerThrusterLevel.Off)
            {
                _leftParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                _rightParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                return;
            }

            ParticleSystem.EmissionModule leftEmission = _leftParticles.emission;
            leftEmission.rateOverTimeMultiplier = GetEmissionRate(level);

            ParticleSystem.EmissionModule rightEmission = _rightParticles.emission;
            rightEmission.rateOverTimeMultiplier = GetEmissionRate(level);

            if (!_leftParticles.isPlaying || !_rightParticles.isPlaying)
            {
                _leftParticles.Play(true);
                _rightParticles.Play(true);
            }
        }

        private float GetEmissionRate(PlayerThrusterLevel level)
        {
            switch (level)
            {
                case PlayerThrusterLevel.Low:
                    return LowEmissionRate;

                case PlayerThrusterLevel.Half:
                    return HalfEmissionRate;

                case PlayerThrusterLevel.Max:
                    return MaxEmissionRate;

                default:
                    return 0f;
            }
        }
    }
}
