namespace Game.Core.Input
{
    public readonly struct PlayerInputState
    {
        public float Turn { get; }
        public float Thrust { get; }
        public float Brake { get; }

        public bool FireBulletHeld { get; }
        public bool FireLaserPressed { get; }

        public PlayerInputState(float turn, float thrust, float brake, bool fireBulletHeld, bool fireLaserPressed)
        {
            Turn = turn;
            Thrust = thrust;
            Brake = brake;
            FireBulletHeld = fireBulletHeld;
            FireLaserPressed = fireLaserPressed;
        }
    }
}
