using System;
using Game.Core.Configuration;
using Game.Core.Input;
using Game.Core.Player;
using Game.Gameplay.Projectiles;
using UnityEngine;
using Zenject;

namespace Game.Gameplay.Player
{
    public sealed class PlayerWeapon : MonoBehaviour
    {
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private ProjectilePool _projectilePool;

        private IGameConfigProvider _configProvider;
        private PlayerInputStateProvider _inputProvider;
        private PlayerInvulnerability _invulnerability;

        private float _timeUntilNextShot;

        [Inject]
        private void Construct(IGameConfigProvider configProvider, PlayerInputStateProvider inputProvider,
            PlayerInvulnerability invulnerability)
        {
            _configProvider = configProvider ?? throw new ArgumentNullException(nameof(configProvider));

            _inputProvider = inputProvider ?? throw new ArgumentNullException(nameof(inputProvider));

            _invulnerability = invulnerability ?? throw new ArgumentNullException(nameof(invulnerability));
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

            if (_invulnerability.IsActive)
            {
                return;
            }

            if (_timeUntilNextShot > 0)
            {
                return;
            }

            PlayerInputState input = _inputProvider.Current;

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
