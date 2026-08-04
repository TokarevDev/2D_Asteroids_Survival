using System;
using Game.Core.Physics;
using Zenject;

namespace Game.Gameplay.Player
{
    public sealed class PlayerPhysicsViewSynchronizer : IFixedTickable
    {
        private readonly PlayerPhysicsController _controller;
        private readonly PlayerPhysicsView _view;

        public PlayerPhysicsViewSynchronizer(PlayerPhysicsController controller, PlayerPhysicsView view)
        {
            _controller = controller ?? throw new ArgumentNullException(nameof(controller));
            _view = view ?? throw new ArgumentNullException(nameof(view));
        }

        public void FixedTick()
        {
            CustomPhysicsBody2D body = _controller.Body;

            _view.ApplyState(body.Position, body.RotationDegrees);
        }
    }
}
