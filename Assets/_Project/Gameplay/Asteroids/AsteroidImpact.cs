using UnityEngine;

namespace Game.Gameplay
{
    [RequireComponent(typeof(Asteroid))]
    public sealed class AsteroidImpact : MonoBehaviour
    {
        [SerializeField] private Asteroid _asteroid;

        private void Awake()
        {
            if (_asteroid != null)
            {
                return;
            }

            Debug.LogError("Asteroid reference is missing", this);
            enabled = false;
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
    }
}
