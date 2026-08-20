using System;
using Zenject;

namespace Game.Gameplay.Player
{
    public sealed class PlayerThrustVisualController : ITickable, IDisposable
    {
        private const float MinimumThrust = 0.01f;
        private const float HalfSpeedThreshold = 0.35f;
        private const float MaxSpeedThreshold = 0.75f;

        private readonly PlayerPhysicsController _physicsController;
        private readonly PlayerThrustView _view;

        public PlayerThrustVisualController(PlayerPhysicsController physicsController, PlayerThrustView view)
        {
            _physicsController = physicsController ?? throw new ArgumentNullException(nameof(physicsController));

            _view = view ?? throw new ArgumentNullException(nameof(view));
        }

        public void Tick()
        {
            _view.SetLevel(GetLevel());
        }

        public void Dispose()
        {
            _view.SetLevel(PlayerThrusterLevel.Off);
        }

        private PlayerThrusterLevel GetLevel()
        {
            if (_physicsController.CurrentThrust <= MinimumThrust)
            {
                return PlayerThrusterLevel.Off;
            }

            float normalizedSpeed = _physicsController.NormalizedSpeed;

            if (normalizedSpeed < HalfSpeedThreshold)
            {
                return PlayerThrusterLevel.Low;
            }

            if (normalizedSpeed < MaxSpeedThreshold)
            {
                return PlayerThrusterLevel.Half;
            }

            return PlayerThrusterLevel.Max;
        }
    }
}
