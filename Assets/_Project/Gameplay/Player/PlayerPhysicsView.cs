using UnityEngine;

namespace Game.Gameplay.Player
{
    [DisallowMultipleComponent]
    public sealed class PlayerPhysicsView : MonoBehaviour
    {
        private Transform _cachedTransform;

        public Vector2 Position => _cachedTransform.position;
        public float RotationDegrees => _cachedTransform.eulerAngles.z;

        private void Awake()
        {
            _cachedTransform = transform;
        }

        public void ApplyState(Vector2 position, float rotationDegrees)
        {
            Vector3 currentPosition = _cachedTransform.position;
            currentPosition.x = position.x;
            currentPosition.y = position.y;

            _cachedTransform.SetPositionAndRotation(
                currentPosition, Quaternion.Euler(0f, 0f, rotationDegrees));
        }
    }
}
