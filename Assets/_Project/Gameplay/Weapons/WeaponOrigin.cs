using UnityEngine;

namespace Game.Gameplay.Weapons
{
    public sealed class WeaponOrigin : MonoBehaviour
    {
        private Transform _cachedTransform;

        public Vector2 Position => _cachedTransform.position;
        public Vector2 Direction => _cachedTransform.up;
        public float RotationDegrees => _cachedTransform.eulerAngles.z;

        private void Awake()
        {
            _cachedTransform = transform;
        }
    }
}
