using System;
using Game.Core.World;
using Zenject;

namespace Game.Gameplay.Player
{
    public sealed class PlayerWorldWrapController : IFixedTickable
    {
        private readonly PlayerPhysicsController _physicsController;
        private readonly ToroidalWorld2D _world;

        public PlayerWorldWrapController(PlayerPhysicsController physicsController, ToroidalWorld2D world)
        {
            _physicsController = physicsController ?? throw new ArgumentNullException(nameof(physicsController));

            _world = world ?? throw new ArgumentNullException(nameof(world));
        }

        public void FixedTick()
        {
            _world.Wrap(_physicsController.Body);
        }
    }
}
