using UnityEngine;

namespace Game.Gameplay
{
    [RequireComponent(typeof(Asteroid))]
    public sealed class AsteroidImpact : MonoBehaviour
    {
        [SerializeField] private Asteroid _asteroid;

        private void Awake()
        {
            if (!ValidateSerializedReferences())
            {
                enabled = false;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent(out IDamageable damageable))
            {
                return;
            }

            int impactDamage = _asteroid.CurrentHealth;

            damageable.TakeDamage(impactDamage);
            _asteroid.Kill();
        }

        private bool ValidateSerializedReferences()
        {
            if (_asteroid != null)
            {
                return true;
            }

            Debug.LogError("Asteroid reference is missing", this);
            return false;
        }
    }
}
