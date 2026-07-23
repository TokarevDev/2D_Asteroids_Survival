using UnityEngine;

namespace Game.Gameplay
{
    [CreateAssetMenu(fileName = "AsteroidAnimation", menuName = "Game/Asteroids/Animation Config")]
    public sealed class AsteroidAnimationConfig : ScriptableObject
    {
        [SerializeField] private Sprite[] _frames;

        [SerializeField, Min(0.1f)] private float _framesPerSecond = 20f;

        public int FrameCount => _frames?.Length ?? 0;
        public float FrameDuration => 1f / Mathf.Max(0.1f, _framesPerSecond);

        public Sprite GetFrame(int index)
        {
            return _frames[index];
        }
    }
}
