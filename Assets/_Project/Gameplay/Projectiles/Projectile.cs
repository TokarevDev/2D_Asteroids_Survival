using UnityEngine;

namespace Game.Gameplay.Projectiles
{
    public sealed class Projectile : MonoBehaviour
    {
        [SerializeField] private ProjectilePhysicsView _physicsView;

        public ProjectilePhysicsView PhysicsView => _physicsView;

        private void Awake()
        {
            if (!ValidateSerializedReferences())
            {
                enabled = false;
            }
        }

        private bool ValidateSerializedReferences()
        {
            if (_physicsView != null)
            {
                return true;
            }

            Debug.LogError("Projectile physics view reference is missing", this);

            return false;
        }
    }
}
