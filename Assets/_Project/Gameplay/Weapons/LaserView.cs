using UnityEngine;

namespace Game.Gameplay.Weapons
{
    public sealed class LaserView : MonoBehaviour
    {
        private const int StartPointIndex = 0;
        private const int EndPointIndex = 1;
        private const int LinePointCount = 2;

        [SerializeField] private LineRenderer _lineRenderer;

        private float _maximumWidth;

        private void Awake()
        {
            if (_lineRenderer == null)
            {
                Debug.LogError("Laser LineRenderer reference is missing", this);
                enabled = false;
                return;
            }

            _lineRenderer.useWorldSpace = true;
            _lineRenderer.positionCount = LinePointCount;
            _maximumWidth = _lineRenderer.widthMultiplier;
            _lineRenderer.enabled = false;
        }

        public void Show(Vector2 start, Vector2 end)
        {
            _lineRenderer.SetPosition(StartPointIndex, start);
            _lineRenderer.SetPosition(EndPointIndex, end);
            _lineRenderer.widthMultiplier = _maximumWidth;
            _lineRenderer.enabled = true;
        }

        public void SetWidthScale(float normalizedScale)
        {
            _lineRenderer.widthMultiplier = _maximumWidth * Mathf.Clamp01(normalizedScale);
        }

        public void Hide()
        {
            _lineRenderer.enabled = false;
        }
    }
}
