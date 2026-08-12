using System;
using UnityEngine;

namespace Game.Gameplay.Player
{
    public sealed class PlayerCollisionVfx : MonoBehaviour
    {
        public event Action<PlayerCollisionVfx> Finished;

        [SerializeField] private ParticleSystem _particles;

        private void Awake()
        {
            if (_particles == null)
            {
                Debug.LogError("Collision VFX particle system reference is missing", this);
                enabled = false;
                return;
            }

            ParticleSystem.MainModule main = _particles.main;
            main.stopAction = ParticleSystemStopAction.Callback;
        }

        public void Play(Vector2 position)
        {
            transform.position = position;
            gameObject.SetActive(true);

            _particles.Clear();
            _particles.Play(true);
        }

        private void OnParticleSystemStopped()
        {
            Finished?.Invoke(this);
        }
    }
}
