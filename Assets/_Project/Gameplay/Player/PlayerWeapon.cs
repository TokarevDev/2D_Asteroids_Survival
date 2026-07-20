using UnityEngine;

namespace Game.Gameplay
{
    public sealed class PlayerWeapon : MonoBehaviour
    {
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private ProjectilePool _projectilePool;

        [SerializeField, Min(0.01f)] private float _fireInterval = 0.2f;
        [SerializeField, Min(0.01f)] private float _projectileSpeed = 15f;
        [SerializeField, Min(1)] private int _damage = 1;

        private float _timeUntilNextShot;


        private void Awake()
        {
            if (_spawnPoint == null)
            {
                Debug.LogError("Projectile spawn point reference is missing", this);
                enabled = false;
                return;
            }

            if (_projectilePool == null)
            {
                Debug.LogError("Projectile pool reference is missing", this);
                enabled = false;
            }
        }

        private void Update()
        {
            _timeUntilNextShot -= Time.deltaTime;

            if (_timeUntilNextShot > 0)
            {
                return;
            }

            Shoot();
            _timeUntilNextShot = _fireInterval;
        }

        private void Shoot()
        {
            Projectile projectile = _projectilePool.Get(_spawnPoint.position);

            projectile.Launch(Vector2.up, _projectileSpeed, _damage);
        }
    }
}
