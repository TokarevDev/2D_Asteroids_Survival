using System;
using Game.Core.Configuration;
using Game.Core.Input;
using UnityEngine;
using Zenject;

namespace Game.Gameplay
{
    public sealed class PlayerWeapon : MonoBehaviour
    {
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private ProjectilePool _projectilePool;

        private IGameConfigProvider _configProvider;
        private IPlayerInputStrategy _inputStrategy;

        private float _timeUntilNextShot;

        [Inject]
        private void Construct(IGameConfigProvider configProvider, IPlayerInputStrategy inputStrategy)
        {
            _configProvider = configProvider ?? throw new ArgumentNullException(nameof(configProvider));
            _inputStrategy = inputStrategy ?? throw new ArgumentNullException(nameof(inputStrategy));
        }

        private void Awake()
        {
            if (!ValidateSerializedReferences())
            {
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

            PlayerInputState input = _inputStrategy.Read();

            if (!input.FireBulletHeld)
            {
                return;
            }

            Shoot();
            _timeUntilNextShot = 1f / _configProvider.Player.ShotsPerSecond;
        }

        private void Shoot()
        {
            _projectilePool.TrySpawn(_spawnPoint.position, _spawnPoint.up, _spawnPoint.eulerAngles.z);
        }

        private bool ValidateSerializedReferences()
        {
            bool isValid = true;

            if (_spawnPoint == null)
            {
                Debug.LogError("Projectile spawn point reference is missing", this);
                isValid = false;
            }

            if (_projectilePool == null)
            {
                Debug.LogError("Projectile pool reference is missing", this);
                isValid = false;
            }

            return isValid;
        }
    }
}
