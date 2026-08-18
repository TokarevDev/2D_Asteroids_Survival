using System;
using Game.Gameplay.Combat;
using UnityEngine;

namespace Game.Gameplay.Enemies.Ufo
{
    [RequireComponent(typeof(EnemyPhysicsView))]
    public sealed class Ufo : MonoBehaviour, IDamageable
    {
        public event Action<Ufo, DeathSource> Died;

        [SerializeField] private EnemyPhysicsView _physicsView;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private UfoVisualVariantSelector _visualVariantSelector;

        private readonly Health _health = new();

        private DeathSource _deathSource;

        public EnemyPhysicsView PhysicsView => _physicsView;
        public int CurrentHealth => _health.CurrentHealth;

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

        public void Initialize(int maxHealth)
        {
            if (maxHealth <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxHealth), maxHealth,
                    "Maximum health must be greater than zero");
            }

            _deathSource = DeathSource.Environment;
            _health.Initialize(maxHealth);
            _visualVariantSelector.ApplyRandomVariant();
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

        private void OnHealthDied()
        {
            Died?.Invoke(this, _deathSource);
        }

        private bool ValidateSerializedReferences()
        {
            bool isValid = true;
            if (_physicsView == null)
            {
                Debug.LogError("UFO physics view reference is missing", this);
                isValid = false;
            }

            if (_spriteRenderer == null)
            {
                Debug.LogError("UFO sprite renderer reference is missing", this);
                isValid = false;
            }

            if (_visualVariantSelector == null)
            {
                Debug.LogError("UFO visual variant selector reference is missing", this);
                isValid = false;
            }
            else if (!_visualVariantSelector.IsConfigured)
            {
                Debug.LogError("UFO visual variants are not configured", this);
                isValid = false;
            }

            return isValid;
        }
    }
}
