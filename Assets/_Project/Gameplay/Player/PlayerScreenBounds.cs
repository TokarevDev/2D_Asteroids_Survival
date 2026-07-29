using UnityEngine;
using Zenject;

namespace Game.Gameplay
{
    public sealed class PlayerScreenBounds : MonoBehaviour
    {
        [SerializeField] private Collider2D _collider2D;

        private Camera _camera;

        private Vector3 _viewportMin;
        private Vector3 _viewportMax;

        private Vector2 _minAllowedPosition;
        private Vector2 _maxAllowedPosition;

        private float _cachedAspect;
        private float _cachedOrthographicSize;

        [Inject]
        private void Construct(CameraProvider cameraProvider)
        {
            _camera = cameraProvider.Camera;
        }

        private void Awake()
        {
            if (!ValidateSerializedReferences())
            {
                enabled = false;
                return;
            }

            RefreshBounds();
        }

        public Vector2 Clamp(Vector2 desiredPosition)
        {
            RefreshBoundsIfNeeded();

            float clampedX = Mathf.Clamp(
                desiredPosition.x,
                _minAllowedPosition.x,
                _maxAllowedPosition.x);

            float clampedY = Mathf.Clamp(
                desiredPosition.y,
                _minAllowedPosition.y,
                _maxAllowedPosition.y);

            return new Vector2(clampedX, clampedY);
        }

        private void RefreshBounds()
        {
            _viewportMin = _camera.ViewportToWorldPoint(Vector3.zero);
            _viewportMax = _camera.ViewportToWorldPoint(Vector3.one);

            Bounds colliderBounds = _collider2D.bounds;
            Vector2 extents = colliderBounds.extents;

            Vector2 centerOffset = colliderBounds.center - transform.position;

            _minAllowedPosition = new Vector2(
                _viewportMin.x + extents.x - centerOffset.x,
                _viewportMin.y + extents.y - centerOffset.y);

            _maxAllowedPosition = new Vector2(
                _viewportMax.x - extents.x - centerOffset.x,
                _viewportMax.y - extents.y - centerOffset.y);

            _cachedAspect = _camera.aspect;
            _cachedOrthographicSize = _camera.orthographicSize;
        }

        private void RefreshBoundsIfNeeded()
        {
            bool aspectChanged = !Mathf.Approximately(_cachedAspect, _camera.aspect);

            bool sizeChanged = !Mathf.Approximately(_cachedOrthographicSize, _camera.orthographicSize);

            if (!aspectChanged && !sizeChanged)
            {
                return;
            }

            RefreshBounds();
        }

        private bool ValidateSerializedReferences()
        {
            bool isValid = true;

            if (_collider2D == null)
            {
                Debug.LogError("Collider2D reference is missing", this);
                isValid = false;
            }

            return isValid;
        }
    }
}
