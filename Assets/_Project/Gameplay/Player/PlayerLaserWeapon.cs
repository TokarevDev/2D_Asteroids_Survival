using System;
using Game.Core.Input;
using Game.Core.Player;
using Game.Core.Weapons;
using Game.Gameplay.Weapons;
using Zenject;

namespace Game.Gameplay.Player
{
    public sealed class PlayerLaserWeapon : ITickable
    {
        private readonly PlayerInputStateProvider _inputProvider;
        private readonly PlayerInvulnerability _invulnerability;
        private readonly WeaponOrigin _weaponOrigin;
        private readonly LaserChargeMagazine _chargeMagazine;
        private readonly LaserShotService _shotService;

        public PlayerLaserWeapon(PlayerInputStateProvider inputProvider, PlayerInvulnerability invulnerability,
            WeaponOrigin weaponOrigin, LaserChargeMagazine chargeMagazine, LaserShotService shotService)
        {
            _inputProvider = inputProvider ?? throw new ArgumentNullException(nameof(inputProvider));

            _invulnerability = invulnerability ?? throw new ArgumentNullException(nameof(invulnerability));

            _weaponOrigin = weaponOrigin ?? throw new ArgumentNullException(nameof(weaponOrigin));

            _chargeMagazine = chargeMagazine ?? throw new ArgumentNullException(nameof(chargeMagazine));

            _shotService = shotService ?? throw new ArgumentNullException(nameof(shotService));
        }

        public void Tick()
        {
            if (_invulnerability.IsActive)
            {
                return;
            }

            PlayerInputState input = _inputProvider.Current;

            if (!input.FireLaserPressed || !_chargeMagazine.TryConsume())
            {
                return;
            }

            _shotService.Fire(_weaponOrigin.Position, _weaponOrigin.Direction);
        }
    }
}
