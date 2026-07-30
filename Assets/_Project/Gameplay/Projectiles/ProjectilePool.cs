using UnityEngine;

namespace Game.Gameplay
{
    public sealed class ProjectilePool : MonoBehaviour
    {
        [SerializeField] private Projectile _projectilePrefab;
        [SerializeField, Min(1)] private int _initialSize = 30;
        private ObjectPool<Projectile> _pool;

        private void Awake()
        {
            if (!ValidateSerializedReferences())
            {
                enabled = false;
                return;
            }

            _pool = new ObjectPool<Projectile>(CreateProjectile, _initialSize);
        }

        private void OnDestroy()
        {
            if (_pool == null)
            {
                return;
            }

            for (int i = 0; i < _pool.CreatedItems.Count; i++)
            {
                Projectile projectile = _pool.CreatedItems[i];

                if (projectile == null)
                {
                    continue;
                }

                projectile.Hit -= Return;
            }

            _pool.Clear();
        }

        public Projectile Get(Vector2 position)
        {
            Projectile projectile = _pool.Get();

            projectile.transform.SetPositionAndRotation(position, Quaternion.identity);

            projectile.gameObject.SetActive(true);

            return projectile;
        }

        public void Return(Projectile projectile)
        {
            if (projectile == null)
            {
                Debug.LogError("Cannot return a null projectile", this);
                return;
            }

            if (!_pool.Return(projectile))
            {
                Debug.LogWarning("Projectile is already in the pool", projectile);
                return;
            }

            projectile.Stop();
            projectile.gameObject.SetActive(false);
        }

        private Projectile CreateProjectile()
        {
            Projectile projectile = Instantiate(_projectilePrefab, transform);

            projectile.Hit += Return;
            projectile.gameObject.SetActive(false);
            return projectile;
        }

        private bool ValidateSerializedReferences()
        {
            if (_projectilePrefab != null)
            {
                return true;
            }

            Debug.LogError("Projectile prefab reference is missing", this);
            return false;
        }
    }
}
