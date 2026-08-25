using UnityEngine;

namespace Game.Gameplay.Asteroids.Animation
{
    [CreateAssetMenu(fileName = "AsteroidAnimation", menuName = "Game/Asteroids/Animation Config")]
    public sealed class AsteroidAnimationConfig : ScriptableObject
    {
        private const float MinimumFramesPerSecond = 0.1f;

        [SerializeField] private Sprite[] _frames;

        [SerializeField, Min(MinimumFramesPerSecond)]
        private float _framesPerSecond = 20f;

        public int FrameCount => _frames?.Length ?? 0;
        public float FrameDuration => 1f / Mathf.Max(MinimumFramesPerSecond, _framesPerSecond);

        public Sprite GetFrame(int index)
        {
            return _frames[index];
        }
    }
}
