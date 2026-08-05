using System;
using Game.Gameplay.Enemies;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Gameplay
{
    [RequireComponent(typeof(AsteroidMovement)), RequireComponent(typeof(EnemyPhysicsView))]
    public sealed class Asteroid : MonoBehaviour, IDamageable
    {
        public event Action<Asteroid, DeathSource> Died;

        [SerializeField] private EnemyPhysicsView _physicsView;

        [SerializeField] private AsteroidMovement _movement;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private AsteroidSpriteAnimator _spriteAnimator;

        private readonly Health _health = new();

        private AsteroidConfig _config;

        private DeathSource _deathSource;

        public EnemyPhysicsView PhysicsView => _physicsView;

        public int CurrentHealth => _health.CurrentHealth;

        public int ScoreReward => _config.ScoreReward;

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

        public void Initialize(AsteroidConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            _config = config;
            _deathSource = DeathSource.Environment;

            _health.Initialize(config.MaxHealth);
            _spriteAnimator.Stop();

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

            transform.localScale = Vector3.one * config.Scale;
        }

        public void Launch(Vector2 direction)
        {
            if (_config == null)
            {
                throw new InvalidOperationException(
                    "Asteroid must be initialized before launch");
            }

            float angularSpeed = GetAngularSpeed();

            _movement.Launch(
                direction,
                _config.MovementSpeed,
                angularSpeed);
        }

        public void Kill()
        {
            _deathSource = DeathSource.Environment;
            _health.TakeDamage(_health.CurrentHealth);
        }

        public void Stop()
        {
            _movement.Stop();
            _spriteAnimator.Stop();
        }

        private float GetAngularSpeed()
        {
            if (_config.UseFrameAnimation)
            {
                return 0f;
            }

            float angularSpeed = Random.Range(
                _config.MinAngularSpeed,
                _config.MaxAngularSpeed);

            bool rotateClockwise = Random.value < 0.5f;

            return rotateClockwise
                ? -angularSpeed
                : angularSpeed;
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

            if (_movement == null)
            {
                Debug.LogError("Asteroid movement reference is missing", this);
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

            return isValid;
        }
    }
}
