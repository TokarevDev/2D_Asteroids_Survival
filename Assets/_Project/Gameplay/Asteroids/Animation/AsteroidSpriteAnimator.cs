using System;
using UnityEngine;

namespace Game.Gameplay
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class AsteroidSpriteAnimator : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;

        private AsteroidAnimationConfig _animation;
        private float _frameDuration;
        private float _elapsedTime;
        private int _frameIndex;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            enabled = false;
        }

        private void Update()
        {
            _elapsedTime += Time.deltaTime;

            if (_elapsedTime < _frameDuration)
            {
                return;
            }

            AdvanceFrames();
        }

        public void Play(AsteroidAnimationConfig animation)
        {
            ValidateAnimation(animation);

            _animation = animation;
            _frameDuration = animation.FrameDuration;
            _elapsedTime = 0f;
            _frameIndex = 0;

            _spriteRenderer.sprite = animation.GetFrame(0);
            enabled = true;
        }

        public void ShowFrame(AsteroidAnimationConfig animation, int frameIndex)
        {
            ValidateAnimation(animation);

            if (frameIndex < 0 || frameIndex >= animation.FrameCount)
            {
                throw new ArgumentOutOfRangeException(nameof(frameIndex));
            }

            Stop();

            _spriteRenderer.sprite = animation.GetFrame(frameIndex);
        }

        public void Stop()
        {
            _animation = null;
            _elapsedTime = 0f;
            enabled = false;
        }

        private void AdvanceFrames()
        {
            int framesToAdvance = Mathf.FloorToInt(_elapsedTime / _frameDuration);

            _elapsedTime -= framesToAdvance * _frameDuration;
            _frameIndex = (_frameIndex + framesToAdvance) % _animation.FrameCount;
            _spriteRenderer.sprite = _animation.GetFrame(_frameIndex);
        }

        private static void ValidateAnimation(AsteroidAnimationConfig animation)
        {
            if (animation == null)
            {
                throw new ArgumentNullException(nameof(animation));
            }

            if (animation.FrameCount == 0)
            {
                throw new InvalidOperationException(
                    $"Animation '{animation.name}' has no frames");
            }
        }
    }
}
