using Game.Gameplay.Asteroids.Animation;
using UnityEngine;

namespace Game.Gameplay.Asteroids
{
    [CreateAssetMenu(fileName = "AsteroidConfig", menuName = "Game/Asteroids/Asteroid Config")]
    public sealed class AsteroidConfig : ScriptableObject
    {
        [SerializeField] private Sprite _sprite;
        [SerializeField] private AsteroidAnimationConfig[] _animationVariants;

        [SerializeField, Min(0.01f)] private float _scale = 1f;

        [Header("Visual Motion"), SerializeField]
        private bool _useFrameAnimation = true;

        [SerializeField, Min(0f)] private float _minAngularSpeed = 15f;
        [SerializeField, Min(0f)] private float _maxAngularSpeed = 35f;

        public bool UseFrameAnimation => _useFrameAnimation;
        public float MinAngularSpeed => _minAngularSpeed;
        public float MaxAngularSpeed => _maxAngularSpeed;

        public Sprite Sprite => _sprite;
        public float Scale => _scale;
        public int AnimationVariantCount => _animationVariants?.Length ?? 0;

        public AsteroidAnimationConfig GetAnimationVariant(int index)
        {
            return _animationVariants[index];
        }

        private void OnValidate()
        {
            if (_maxAngularSpeed < _minAngularSpeed)
            {
                _maxAngularSpeed = _minAngularSpeed;
            }
        }
    }
}
