using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class ProjectilePool : MonoBehaviour
    {
        [SerializeField] private Projectile _projectilePrefab;
        [SerializeField, Min(1)] private int _initialSize = 30;

        private readonly Queue<Projectile> _availableProjectiles = new();
        private readonly HashSet<Projectile> _availableProjectileSet = new();

        private void Awake()
        {
            if (_projectilePrefab == null)
            {
                Debug.LogError("Projectile prefab reference is missing", this);
                enabled = false;
                return;
            }

            Prewarm();
        }

        public Projectile Get(Vector2 position)
        {
            Projectile projectile;
            if (_availableProjectiles.Count > 0)
            {
                projectile = _availableProjectiles.Dequeue();
                _availableProjectileSet.Remove(projectile);
            }
            else
            {
                projectile = CreateProjectile();
            }

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

            if (!_availableProjectileSet.Add(projectile))
            {
                Debug.LogWarning("Projectile is already in the pool", projectile);
                return;
            }

            projectile.Stop();
            projectile.gameObject.SetActive(false);

            _availableProjectiles.Enqueue(projectile);
        }

        private void Prewarm()
        {
            for (int i = 0; i < _initialSize; i++)
            {
                Projectile projectile = CreateProjectile();
                Return(projectile);
            }
        }

        private Projectile CreateProjectile()
        {
            Projectile projectile = Instantiate(_projectilePrefab, transform);

            projectile.Hit += Return;
            projectile.gameObject.SetActive(false);
            return projectile;
        }
    }
}
