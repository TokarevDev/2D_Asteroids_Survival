using UnityEngine;

namespace Game.Gameplay
{
    [CreateAssetMenu(fileName = "AsteroidConfig", menuName = "Game/Asteroids/Asteroid Config")]
    public sealed class AsteroidConfig : ScriptableObject
    {
        [SerializeField, Min(1)] private int _maxHealth = 1;
        [SerializeField, Min(0f)] private float _movementSpeed = 1f;
        [SerializeField, Min(0)] private int _scoreReward = 100;


        [SerializeField] private Sprite _sprite;
        [SerializeField] private AsteroidAnimationConfig[] _animationVariants;

        [SerializeField, Min(0.01f)] private float _scale = 1f;

        [Header("Visual Motion")] [SerializeField]
        private bool _useFrameAnimation = true;

        [SerializeField, Min(0f)] private float _minAngularSpeed = 15f;
        [SerializeField, Min(0f)] private float _maxAngularSpeed = 35f;

        public bool UseFrameAnimation => _useFrameAnimation;
        public float MinAngularSpeed => _minAngularSpeed;
        public float MaxAngularSpeed => _maxAngularSpeed;

        public int MaxHealth => _maxHealth;
        public float MovementSpeed => _movementSpeed;
        public int ScoreReward => _scoreReward;
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
