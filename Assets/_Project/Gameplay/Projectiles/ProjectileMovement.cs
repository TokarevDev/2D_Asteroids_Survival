using UnityEngine;

namespace Game.Gameplay
{
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class ProjectileMovement : MonoBehaviour
    {
        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public void Launch(Vector2 direction, float speed)
        {
            _rigidbody.velocity = direction.normalized * speed;
        }

        public void Stop()
        {
            _rigidbody.velocity = Vector2.zero;
        }
    }
}
