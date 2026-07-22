using System;
using UnityEngine;

namespace Game.Gameplay
{
    [RequireComponent(typeof(AsteroidMovement))]
    public sealed class Asteroid : MonoBehaviour, IDamageable
    {
        public event Action<Asteroid> Died;
        public event Action<int> DestroyedByPlayer;
        public int CurrentHealth => _health.CurrentHealth;

        [SerializeField] private AsteroidMovement _movement;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private readonly Health _health = new();

        private bool _shouldGrantScore;

        private AsteroidConfig _config;

        private void Awake()
        {
            _health.Died += OnHealthDied;
            if (_movement == null)
            {
                Debug.LogError("Asteroid movement reference is missing", this);
                enabled = false;
                return;
            }

            if (_spriteRenderer == null)
            {
                Debug.LogError("Asteroid sprite renderer reference is missing", this);
                enabled = false;
            }
        }

        private void OnDestroy()
        {
            _health.Died -= OnHealthDied;
        }

        public void Initialize(AsteroidConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            _config = config;
            _shouldGrantScore = false;

            _health.Initialize(config.MaxHealth);
            _spriteRenderer.sprite = config.Sprite;
            transform.localScale = Vector3.one * config.Scale;
        }

        public void Launch(Vector2 direction)
        {
            if (_config == null)
            {
                throw new InvalidOperationException("Asteroid must be initialized before launch");
            }

            _movement.Launch(direction, _config.MovementSpeed);
        }

        public void TakeDamage(int damage)
        {
            _shouldGrantScore = true;
            _health.TakeDamage(damage);
        }

        public void Kill()
        {
            _shouldGrantScore = false;
            _health.TakeDamage(_health.CurrentHealth);
        }

        public void Stop()
        {
            _movement.Stop();
        }

        private void OnHealthDied()
        {
            if (_shouldGrantScore)
            {
                DestroyedByPlayer?.Invoke(_config.ScoreReward);
            }

            Died?.Invoke(this);
        }
    }
}
