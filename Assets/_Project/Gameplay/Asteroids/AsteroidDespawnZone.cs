using UnityEngine;

namespace Game.Gameplay
{
    [RequireComponent(typeof(BoxCollider2D))]
    public sealed class AsteroidDespawnZone : MonoBehaviour
    {
        [SerializeField] private AsteroidPool _asteroidPool;

        private void Awake()
        {
            if (!ValidateSerializedReferences())
            {
                enabled = false;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent(out Asteroid asteroid))
            {
                return;
            }

            _asteroidPool.Return(asteroid);
        }

        private bool ValidateSerializedReferences()
        {
            if (_asteroidPool != null)
            {
                return true;
            }

            Debug.LogError("Asteroid pool reference is missing", this);
            return false;
        }
    }
}
