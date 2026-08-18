using UnityEngine;

namespace Game.Gameplay.Asteroids.Animation
{
    [DisallowMultipleComponent]
    public sealed class AsteroidVisualRotator : MonoBehaviour
    {
        private Transform _cachedTransform;
        private float _angularSpeed;

        private void Awake()
        {
            _cachedTransform = transform;
            enabled = false;
        }

        private void Update()
        {
            _cachedTransform.Rotate(0f, 0f, _angularSpeed * Time.deltaTime, Space.Self);
        }

        public void Play(float angularSpeed)
        {
            _angularSpeed = angularSpeed;
            _cachedTransform.localRotation = Quaternion.identity;
            enabled = !Mathf.Approximately(angularSpeed, 0f);
        }

        public void Stop()
        {
            _angularSpeed = 0f;
            _cachedTransform.localRotation = Quaternion.identity;
            enabled = false;
        }
    }
}
