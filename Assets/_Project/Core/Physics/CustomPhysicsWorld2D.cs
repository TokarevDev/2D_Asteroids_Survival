using System;
using System.Collections.Generic;

namespace Game.Core.Physics
{
    public sealed class CustomPhysicsWorld2D
    {
        private readonly List<CustomPhysicsBody2D> _bodies = new();
        private readonly CustomPhysicsIntegrator2D _integrator;

        public int BodyCount => _bodies.Count;

        public CustomPhysicsWorld2D(CustomPhysicsIntegrator2D integrator)
        {
            _integrator = integrator ?? throw new ArgumentNullException(nameof(integrator));
        }

        public bool Register(CustomPhysicsBody2D body)
        {
            if (body == null)
            {
                throw new ArgumentNullException(nameof(body));
            }

            if (_bodies.Contains(body))
            {
                return false;
            }

            _bodies.Add(body);
            return true;
        }

        public bool Unregister(CustomPhysicsBody2D body)
        {
            if (body == null)
            {
                throw new ArgumentNullException(nameof(body));
            }

            return _bodies.Remove(body);
        }

        public void Step(float deltaTime)
        {
            if (deltaTime < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(deltaTime), deltaTime, "Delta time cannot be negative");
            }

            for (int i = 0; i < _bodies.Count; i++)
            {
                _integrator.Step(_bodies[i], deltaTime);
            }
        }

        public void Clear()
        {
            _bodies.Clear();
        }
    }
}
