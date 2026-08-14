using UnityEngine;

namespace Game.Core.Input
{
    public sealed class MobileInputBuffer
    {
        private bool _laserFireRequested;

        public bool FireBulletHeld { get; private set; }
        public Vector2 MovementDirection { get; private set; }

        public void SetMovementDirection(Vector2 direction)
        {
            MovementDirection = Vector2.ClampMagnitude(direction, 1f);
        }

        public void SetFireBulletHeld(bool isHeld)
        {
            FireBulletHeld = isHeld;
        }

        public void RequestLaserFire()
        {
            _laserFireRequested = true;
        }

        public bool ConsumeLaserFireRequest()
        {
            bool wasRequested = _laserFireRequested;
            _laserFireRequested = false;

            return wasRequested;
        }

        public void Reset()
        {
            MovementDirection = Vector2.zero;
            FireBulletHeld = false;
            _laserFireRequested = false;
        }
    }
}
