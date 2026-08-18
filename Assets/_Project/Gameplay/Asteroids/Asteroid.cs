using System;
using Game.Gameplay.Asteroids.Animation;
using Game.Gameplay.Combat;
using Game.Gameplay.Enemies;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Gameplay.Asteroids
{
    [RequireComponent(typeof(EnemyPhysicsView))]
    public sealed class Asteroid : MonoBehaviour, IDamageable
    {
        public event Action<Asteroid, DeathSource> Died;

        [SerializeField] private EnemyPhysicsView _physicsView;

        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private AsteroidSpriteAnimator _spriteAnimator;
        [SerializeField] private AsteroidVisualRotator _visualRotator;

        private readonly Health _health = new();

        private AsteroidConfig _config;

        private DeathSource _deathSource;

        public EnemyPhysicsView PhysicsView => _physicsView;

        public int CurrentHealth => _health.CurrentHealth;

        private void Awake()
        {
            if (!ValidateSerializedReferences())
            {
                enabled = false;
                return;
            }

            _health.Died += OnHealthDied;
        }

        private void OnDestroy()
        {
            _health.Died -= OnHealthDied;
        }

        public void TakeDamage(int damage)
        {
            _deathSource = DeathSource.Player;
            _health.TakeDamage(damage);
        }

        public void SetSortingOrder(int sortingOrder)
        {
            _spriteRenderer.sortingOrder = sortingOrder;
        }

        public void Initialize(AsteroidConfig config, int maxHealth)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (maxHealth <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxHealth), maxHealth,
                    "Maximum health must be greater than zero");
            }

            _config = config;
            _deathSource = DeathSource.Environment;

            _health.Initialize(maxHealth);
            _spriteAnimator.Stop();
            _visualRotator.Stop();

            if (config.AnimationVariantCount > 0)
            {
                int animationIndex =
                    Random.Range(0, config.AnimationVariantCount);

                AsteroidAnimationConfig animationConfig =
                    config.GetAnimationVariant(animationIndex);

                if (config.UseFrameAnimation)
                {
                    _spriteAnimator.Play(animationConfig);
                }
                else
                {
                    int frameIndex =
                        Random.Range(0, animationConfig.FrameCount);

                    _spriteAnimator.ShowFrame(animationConfig, frameIndex);
                }
            }
            else
            {
                _spriteRenderer.sprite = config.Sprite;
            }

            if (!config.UseFrameAnimation)
            {
                StartVisualRotation(config);
            }

            transform.localScale = Vector3.one * config.Scale;
        }

        public void Kill()
        {
            _deathSource = DeathSource.Environment;
            _health.TakeDamage(_health.CurrentHealth);
        }

        public void Stop()
        {
            _spriteAnimator.Stop();
            _visualRotator.Stop();
        }

        private void StartVisualRotation(AsteroidConfig config)
        {
            float angularSpeed = Random.Range(config.MinAngularSpeed, config.MaxAngularSpeed);

            if (Random.value < 0.5f)
            {
                angularSpeed = -angularSpeed;
            }

            _visualRotator.Play(angularSpeed);
        }

        private void OnHealthDied()
        {
            Died?.Invoke(this, _deathSource);
        }

        private bool ValidateSerializedReferences()
        {
            bool isValid = true;

            if (_physicsView == null)
            {
                Debug.LogError("Enemy physics view reference is missing", this);

                isValid = false;
            }

            if (_spriteRenderer == null)
            {
                Debug.LogError("Asteroid sprite renderer reference is missing", this);
                isValid = false;
            }

            if (_spriteAnimator == null)
            {
                Debug.LogError("Asteroid sprite animator reference is missing", this);
                isValid = false;
            }

            if (_visualRotator == null)
            {
                Debug.LogError("Asteroid visual rotator reference is missing", this);
                isValid = false;
            }

            return isValid;
        }
    }
}
