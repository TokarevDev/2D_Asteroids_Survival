using System;
using UnityEngine;
using Random = UnityEngine.Random;

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
        [SerializeField] private AsteroidSpriteAnimator _spriteAnimator;

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
                return;
            }

            if (_spriteAnimator == null)
            {
                Debug.LogError("Asteroid sprite animator reference is missing", this);
                enabled = false;
            }
        }

        private void OnDestroy()
        {
            _health.Died -= OnHealthDied;
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
            _shouldGrantScore = false;

            _health.Initialize(config.MaxHealth);
            _spriteAnimator.Stop();

            if (config.AnimationVariantCount > 0)
            {
                int animationIndex =
                    UnityEngine.Random.Range(0, config.AnimationVariantCount);

                AsteroidAnimationConfig animation =
                    config.GetAnimationVariant(animationIndex);

                if (config.UseFrameAnimation)
                {
                    _spriteAnimator.Play(animation);
                }
                else
                {
                    int frameIndex =
                        UnityEngine.Random.Range(0, animation.FrameCount);

                    _spriteAnimator.ShowFrame(animation, frameIndex);
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
            _spriteAnimator.Stop();
        }

        private float GetAngularSpeed()
        {
            if (_config.UseFrameAnimation)
            {
                return 0f;
            }

            float angularSpeed = UnityEngine.Random.Range(
                _config.MinAngularSpeed,
                _config.MaxAngularSpeed);

            bool rotateClockwise = Random.value < 0.5f;

            return rotateClockwise
                ? -angularSpeed
                : angularSpeed;
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
