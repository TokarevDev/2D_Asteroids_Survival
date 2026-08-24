using System;
using Game.Gameplay.Combat;
using Game.Gameplay.Enemies;
using UnityEngine;

namespace Game.Gameplay.Asteroids
{
    [RequireComponent(typeof(EnemyPhysicsView))]
    public sealed class Asteroid : MonoBehaviour, IDamageable
    {
        public event Action<Asteroid, DeathSource> Died;

        [SerializeField] private EnemyPhysicsView _physicsView;
        [SerializeField] private AsteroidVisual _visual;

        private readonly EnemyHealth _health = new();

        public EnemyPhysicsView PhysicsView => _physicsView;

        public int CurrentHealth => _health.CurrentHealth;

        private void Awake()
        {
            if (!ValidateSerializedReferences())
            {
                enabled = false;
                return;
            }

            _health.Died += OnDied;
        }

        private void OnDestroy()
        {
            _health.Died -= OnDied;
        }

        public void TakeDamage(int damage)
        {
            _health.TakeDamage(damage);
        }

        public void SetSortingOrder(int sortingOrder)
        {
            _visual.SetSortingOrder(sortingOrder);
        }

        public void Initialize(AsteroidConfig config, int maxHealth)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            InitializeHealth(maxHealth);
            InitializeVisual(config);
        }

        public void Kill()
        {
            _health.Kill(DeathSource.Environment);
        }

        public void Stop()
        {
            _visual.Stop();
        }

        private void InitializeHealth(int maxHealth)
        {
            _health.Initialize(maxHealth);
        }

        private void InitializeVisual(AsteroidConfig config)
        {
            _visual.Initialize(config);
        }

        private void OnDied(DeathSource deathSource)
        {
            Died?.Invoke(this, deathSource);
        }

        private bool ValidateSerializedReferences()
        {
            bool isValid = true;

            if (_physicsView == null)
            {
                Debug.LogError("Enemy physics view reference is missing", this);

                isValid = false;
            }

            if (_visual == null)
            {
                Debug.LogError("Asteroid visual reference is missing", this);
                isValid = false;
            }
            else if (!_visual.IsConfigured)
            {
                Debug.LogError("Asteroid visual is not configured", this);
                isValid = false;
            }

            return isValid;
        }
    }
}
