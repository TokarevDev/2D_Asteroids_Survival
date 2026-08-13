using UnityEngine;

namespace Game.Gameplay.Weapons
{
    public sealed class LaserView : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;

        private void Awake()
        {
            if (_lineRenderer == null)
            {
                Debug.LogError("Laser LineRenderer reference is missing", this);
                enabled = false;
                return;
            }

            _lineRenderer.useWorldSpace = true;
            _lineRenderer.positionCount = 2;
            _lineRenderer.enabled = false;
        }

        public void Show(Vector2 start, Vector2 end)
        {
            _lineRenderer.SetPosition(0, start);
            _lineRenderer.SetPosition(1, end);
            _lineRenderer.enabled = true;
        }

        public void Hide()
        {
            _lineRenderer.enabled = false;
        }
    }
}
