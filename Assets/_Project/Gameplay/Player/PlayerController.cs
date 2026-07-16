using Game.Core;
using UnityEngine;
using Zenject;

namespace Game.Gameplay
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField, Min(0f)] private float _moveSpeed = 5f;

        [SerializeField] private PlayerScreenBounds _screenBounds;

        private IInputReader _inputReader;
        private Rigidbody2D _rigidbody;

        [Inject]
        private void Construct(IInputReader inputReader)
        {
            _inputReader = inputReader;
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();

            if (_screenBounds == null)
            {
                Debug.LogError("Player screen bound reference is missing", this);
                enabled = false;
            }
        }

        private void FixedUpdate()
        {
            Vector2 direction = Vector2.ClampMagnitude(_inputReader.MoveDirection, 1f);

            Vector2 desiredVelocity = direction * _moveSpeed;

            Vector2 desiredPosition = _rigidbody.position + desiredVelocity * Time.fixedDeltaTime;

            Vector2 clampedPosition = _screenBounds.Clamp(desiredPosition);

            _rigidbody.velocity = (clampedPosition - _rigidbody.position) / Time.fixedDeltaTime;
        }
    }
}
