using UnityEngine;

namespace Game.Gameplay
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class AsteroidDespawnZone : MonoBehaviour
    {
        [SerializeField] private AsteroidPool _asteroidPool;

        private void Awake()
        {
            if (_asteroidPool != null)
            {
                return;
            }

            Debug.LogError("Asteroid pool reference is missing", this);
            enabled = false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent(out AsteroidMovement asteroid))
            {
                return;
            }

            _asteroidPool.Return(asteroid);
        }
    }
}
