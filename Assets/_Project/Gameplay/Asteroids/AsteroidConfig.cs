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
        [SerializeField, Min(0.01f)] private float _scale = 1f;

        public int MaxHealth => _maxHealth;
        public float MovementSpeed => _movementSpeed;
        public int ScoreReward => _scoreReward;
        public Sprite Sprite => _sprite;
        public float Scale => _scale;
    }
}
