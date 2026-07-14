using Game.Core;
using UnityEngine;
using Zenject;

namespace Game.Gameplay
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField, Min(0f)]
        private float _moveSpeed = 5f;

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
            _rigidbody.velocity = direction * _moveSpeed;
        }
    }
}
