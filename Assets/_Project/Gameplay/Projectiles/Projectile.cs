using System;
using UnityEngine;

namespace Game.Gameplay
{
    [RequireComponent(typeof(ProjectileMovement))]
    public sealed class Projectile : MonoBehaviour
    {
        public event Action<Projectile> Hit;

        [SerializeField] private ProjectileMovement _movement;

        private int _damage;
        private bool _canHit;

        private void Awake()
        {
            if (_movement != null) return;

            Debug.LogError("Projectile movement reference is missing", this);
            enabled = false;
        }

        public void Launch(Vector2 direction, float speed, int damage)
        {
            if (speed <= 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(speed));
            }

            if (damage <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(damage));
            }

            _damage = damage;
            _canHit = true;

            _movement.Launch(direction, speed);
        }

        public void Stop()
        {
            _canHit = false;
            _movement.Stop();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!_canHit) return;

            if (!other.TryGetComponent(out IDamageable damageable)) return;

            _canHit = false;

            damageable.TakeDamage(_damage);
            Hit?.Invoke(this);
        }
    }
}
