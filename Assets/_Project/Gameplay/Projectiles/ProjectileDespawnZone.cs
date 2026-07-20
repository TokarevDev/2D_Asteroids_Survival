using UnityEngine;

namespace Game.Gameplay
{
    [RequireComponent(typeof(BoxCollider2D))]
    public sealed class ProjectileDespawnZone : MonoBehaviour
    {
        [SerializeField] private ProjectilePool _projectilePool;

        private void Awake()
        {
            if (_projectilePool != null)
            {
                return;
            }

            Debug.LogError("Projectile pool reference is missing", this);
            enabled = false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent(out Projectile projectile))
            {
                return;
            }

            _projectilePool.Return(projectile);
        }
    }
}
