using System;

namespace Game.Core.Physics
{
    public sealed class CustomPhysicsIntegrator2D
    {
        public void Step(CustomPhysicsBody2D body, float deltaTime)
        {
            if (body == null)
            {
                throw new ArgumentNullException(nameof(body));
            }

            if (deltaTime < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(deltaTime), deltaTime, "Delta time cannot be negative");
            }

            body.SetPosition(body.Position + body.Velocity * deltaTime);
        }
    }
}
