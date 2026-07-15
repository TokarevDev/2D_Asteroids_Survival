using System;
using UnityEngine;

namespace Game.Gameplay
{
    public class PlayerScreenBounds : MonoBehaviour
    {
        [SerializeField] private Camera _camera;

        [SerializeField] private Collider2D _collider2D;

        private Vector3 _viewportMin;
        private Vector3 _viewportMax;

        private Vector2 _minAllowedPosotion;
        private Vector2 _maxAllowedPostition;


        private void Awake()
        {
            if (!CheckReferences())
            {
                enabled = false;
                return;
            }

            RefreshBounds();
        }

        public Vector2 Clamp(Vector2 desiredPosition)
        {
            float clampedX = Mathf.Clamp(
                desiredPosition.x,
                _minAllowedPosotion.x,
                _maxAllowedPostition.x);

            float clampedY = Mathf.Clamp(
                desiredPosition.y,
                _minAllowedPosotion.y,
                _maxAllowedPostition.y);

            return new Vector2(clampedX, clampedY);
        }

        private bool CheckReferences()
        {
            if (_camera == null)
            {
                Debug.LogError("Camera reference is missing", this);
                return false;
            }

            if (_collider2D == null)
            {
                Debug.LogError("Collider2D reference is missing", this);
                return false;
            }

            return true;
        }

        private void RefreshBounds()
        {
            _viewportMin = _camera.ViewportToWorldPoint(Vector3.zero);
            _viewportMax = _camera.ViewportToWorldPoint(Vector3.one);

            Bounds colliderBounds = _collider2D.bounds;
            Vector2 extens = colliderBounds.extents;

            Vector2 centerOffset = colliderBounds.center - transform.position;

            _minAllowedPosotion = new Vector2(
                _viewportMin.x + extens.x - centerOffset.x,
                _viewportMin.y + extens.y - centerOffset.y);

            _maxAllowedPostition = new Vector2(
                _viewportMax.x - extens.x - centerOffset.x,
                _viewportMax.y - extens.y - centerOffset.y);
        }
    }
}
