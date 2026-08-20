using System;
using Game.Core.Configuration;
using Game.Core.Input;
using Game.Core.Physics;
using Game.Core.Player;
using UnityEngine;
using Zenject;

namespace Game.Gameplay.Player
{
    public sealed class PlayerPhysicsController : IInitializable, IFixedTickable, IDisposable
    {
        private readonly PlayerPhysicsView _view;
        private readonly IGameConfigProvider _configProvider;
        private readonly CustomPhysicsWorld2D _physicsWorld;
        private readonly IPlayerInputStrategy _inputStrategy;
        private readonly PlayerInvulnerability _invulnerability;

        private CustomPhysicsBody2D _body;
        private ShipMovement _movement;

        private float _maxSpeed;

        public CustomPhysicsBody2D Body =>
            _body ?? throw new InvalidOperationException("Player physics body has not been initialized");

        public float CurrentThrust { get; private set; }

        public float NormalizedSpeed => Mathf.Clamp01(Body.Velocity.magnitude / _maxSpeed);

        public PlayerPhysicsController(PlayerPhysicsView view, IGameConfigProvider configProvider,
            IPlayerInputStrategy inputStrategy,
            CustomPhysicsWorld2D physicsWorld, PlayerInvulnerability invulnerability)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));

            _configProvider = configProvider ?? throw new ArgumentNullException(nameof(configProvider));

            _inputStrategy = inputStrategy ?? throw new ArgumentNullException(nameof(inputStrategy));

            _physicsWorld = physicsWorld ?? throw new ArgumentNullException(nameof(physicsWorld));

            _invulnerability = invulnerability ?? throw new ArgumentNullException(nameof(invulnerability));
        }

        public void Initialize()
        {
            PlayerConfig config = _configProvider.Player;

            _maxSpeed = config.MaxSpeed;

            _body = new CustomPhysicsBody2D(
                _view.Position, Vector2.zero, _view.RotationDegrees, config.CollisionRadius, config.Mass);

            if (!_physicsWorld.Register(_body))
            {
                throw new InvalidOperationException("Player physics body is already registered");
            }

            _movement = new ShipMovement(_body, config);
        }

        public void FixedTick()
        {
            if (_movement == null)
            {
                throw new InvalidOperationException("Player movement has not been initialized");
            }

            PlayerInputState input = _invulnerability.IsActive ? default : _inputStrategy.Read();

            CurrentThrust = input.MovementDirection.sqrMagnitude > 0f
                ? input.MovementDirection.magnitude
                : Mathf.Clamp01(input.Thrust);

            _movement.Step(input, Time.fixedDeltaTime);
        }

        public void Dispose()
        {
            if (_body == null)
            {
                return;
            }

            _physicsWorld.Unregister(_body);
            _body = null;
            CurrentThrust = 0f;
            _maxSpeed = 0f;
            _movement = null;
        }
    }
}
