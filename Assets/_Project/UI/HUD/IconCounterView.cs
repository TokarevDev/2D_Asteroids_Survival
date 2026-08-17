using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public sealed class IconCounterView : MonoBehaviour
    {
        [SerializeField] private Image[] _icons;

        private bool _isInitialized;
        private bool _isValid;
        private int _visibleCount = -1;

        public int Capacity => _icons?.Length ?? 0;

        private void Awake()
        {
            EnsureInitialized();
        }

        public void SetVisibleCount(int visibleCount)
        {
            EnsureInitialized();

            if (!_isValid)
            {
                return;
            }

            if (visibleCount < 0 || visibleCount > _icons.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(visibleCount), visibleCount,
                    "Visible icon count must be within the configured capacity");
            }

            if (_visibleCount == visibleCount)
            {
                return;
            }

            _visibleCount = visibleCount;

            for (int i = 0; i < _icons.Length; i++)
            {
                bool shouldBeVisible = i < visibleCount;
                GameObject icon = _icons[i].gameObject;

                if (icon.activeSelf != shouldBeVisible)
                {
                    icon.SetActive(shouldBeVisible);
                }
            }
        }

        private void EnsureInitialized()
        {
            if (_isInitialized)
            {
                return;
            }

            _isInitialized = true;
            _isValid = ValidateSerializedReferences();
        }

        private bool ValidateSerializedReferences()
        {
            if (_icons == null || _icons.Length == 0)
            {
                Debug.LogError("Icon references are missing", this);
                return false;
            }

            for (int i = 0; i < _icons.Length; i++)
            {
                if (_icons[i] != null)
                {
                    continue;
                }

                Debug.LogError($"Icon reference at index {i} is missing", this);
                return false;
            }

            return true;
        }
    }
}
