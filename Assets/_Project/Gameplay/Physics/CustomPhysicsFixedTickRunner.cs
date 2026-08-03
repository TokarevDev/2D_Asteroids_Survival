using System;
using Game.Core.Physics;
using UnityEngine;
using Zenject;

namespace Game.Gameplay.Physics
{
    public sealed class CustomPhysicsFixedTickRunner : IFixedTickable
    {
        private readonly CustomPhysicsWorld2D _physicsWorld;

        public CustomPhysicsFixedTickRunner(CustomPhysicsWorld2D physicsWorld)
        {
            _physicsWorld = physicsWorld ?? throw new ArgumentNullException(nameof(physicsWorld));
        }

        public void FixedTick()
        {
            _physicsWorld.Step(Time.fixedDeltaTime);
        }
    }
}
