using System;
using Game.Gameplay.Asteroids.Animation;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Gameplay.Asteroids
{
    [DisallowMultipleComponent]
    public sealed class AsteroidVisual : MonoBehaviour
    {
        private const float ReverseRotationProbability = 0.5f;

        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private AsteroidSpriteAnimator _spriteAnimator;
        [SerializeField] private AsteroidVisualRotator _visualRotator;

        public bool IsConfigured =>
            _spriteRenderer != null &&
            _spriteAnimator != null &&
            _visualRotator != null;

        private void Awake()
        {
            if (IsConfigured)
            {
                return;
            }

            Debug.LogError("Asteroid visual references are missing", this);
            enabled = false;
        }

        public void Initialize(AsteroidConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (!IsConfigured)
            {
                throw new InvalidOperationException("Asteroid visual is not configured");
            }

            Stop();
            ApplyVariant(config);
            ApplyRotation(config);
            ApplyScale(config);
        }

        public void SetSortingOrder(int sortingOrder)
        {
            _spriteRenderer.sortingOrder = sortingOrder;
        }

        public void Stop()
        {
            _spriteAnimator.Stop();
            _visualRotator.Stop();
        }

        private void ApplyVariant(AsteroidConfig config)
        {
            if (config.AnimationVariantCount == 0)
            {
                _spriteRenderer.sprite = config.Sprite;
                return;
            }

            int animationIndex = Random.Range(0, config.AnimationVariantCount);
            AsteroidAnimationConfig animationConfig = config.GetAnimationVariant(animationIndex);

            if (config.UseFrameAnimation)
            {
                _spriteAnimator.Play(animationConfig);
                return;
            }

            int frameIndex = Random.Range(0, animationConfig.FrameCount);
            _spriteAnimator.ShowFrame(animationConfig, frameIndex);
        }

        private void ApplyRotation(AsteroidConfig config)
        {
            if (config.UseFrameAnimation)
            {
                return;
            }

            float angularSpeed = Random.Range(config.MinAngularSpeed, config.MaxAngularSpeed);

            if (Random.value < ReverseRotationProbability)
            {
                angularSpeed = -angularSpeed;
            }

            _visualRotator.Play(angularSpeed);
        }

        private void ApplyScale(AsteroidConfig config)
        {
            transform.localScale = Vector3.one * config.Scale;
        }
    }
}
