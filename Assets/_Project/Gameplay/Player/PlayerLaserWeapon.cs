using System;
using Game.Core.Input;
using Game.Core.Player;
using Game.Core.Weapons;
using Game.Gameplay.Weapons;
using UnityEngine;
using Zenject;

namespace Game.Gameplay.Player
{
    public sealed class PlayerLaserWeapon : ITickable
    {
        private readonly IPlayerInputStrategy _inputStrategy;
        private readonly PlayerInvulnerability _invulnerability;
        private readonly PlayerPhysicsController _physicsController;
        private readonly LaserChargeMagazine _chargeMagazine;
        private readonly LaserShotService _shotService;

        public PlayerLaserWeapon(IPlayerInputStrategy inputStrategy, PlayerInvulnerability invulnerability,
            PlayerPhysicsController physicsController, LaserChargeMagazine chargeMagazine, LaserShotService shotService)
        {
            _inputStrategy = inputStrategy ?? throw new ArgumentNullException(nameof(inputStrategy));

            _invulnerability = invulnerability ?? throw new ArgumentNullException(nameof(invulnerability));

            _physicsController = physicsController ?? throw new ArgumentNullException(nameof(physicsController));

            _chargeMagazine = chargeMagazine ?? throw new ArgumentNullException(nameof(chargeMagazine));

            _shotService = shotService ?? throw new ArgumentNullException(nameof(shotService));
        }

        public void Tick()
        {
            if (_invulnerability.IsActive)
            {
                return;
            }

            PlayerInputState input = _inputStrategy.Read();

            if (!input.FireLaserPressed || !_chargeMagazine.TryConsume())
            {
                return;
            }

            float rotationRadians = _physicsController.Body.RotationDegrees * Mathf.Deg2Rad;
            Vector2 direction = new(-Mathf.Sin(rotationRadians), Mathf.Cos(rotationRadians));

            _shotService.Fire(_physicsController.Body.Position, direction);
        }
    }
}
