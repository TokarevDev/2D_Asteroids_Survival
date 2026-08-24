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
        [SerializeField] private UfoVisualVariantSelector _visualVariantSelector;

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

        public void Initialize(int maxHealth)
        {
            _health.Initialize(maxHealth);
            _visualVariantSelector.ApplyRandomVariant();
        }

        public void TakeDamage(int damage)
        {
            _health.TakeDamage(damage);
        }

        public void SetSortingOrder(int sortingOrder)
        {
            _visualVariantSelector.SetSortingOrder(sortingOrder);
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
                Debug.LogError("UFO physics view reference is missing", this);
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
