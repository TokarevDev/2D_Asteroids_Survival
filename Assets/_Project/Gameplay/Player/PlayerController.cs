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
        }

        private void FixedUpdate()
        {
            Vector2 direction = Vector2.ClampMagnitude(_inputReader.MoveDirection, 1f);

            Vector2 desiredVelosity = direction * _moveSpeed;

            Vector2 desiredPosition = _rigidbody.position + desiredVelosity * Time.fixedDeltaTime;

            Vector2 clampPosition = _screenBounds.Clamp(desiredPosition);

            _rigidbody.velocity = (clampPosition - _rigidbody.position) / Time.fixedDeltaTime;
        }
    }
}
