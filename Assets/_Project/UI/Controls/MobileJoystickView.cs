using System;
using Game.Core.Input;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Game.UI.Controls
{
    public sealed class MobileJoystickView : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        private const int NoPointer = int.MinValue;
        private const float JoystickDeadZone = 0.15f;

        [SerializeField] private RectTransform _background;
        [SerializeField] private RectTransform _handle;

        private MobileInputBuffer _inputBuffer;
        private int _activePointerId = NoPointer;

        [Inject]
        private void Construct(MobileInputBuffer inputBuffer)
        {
            _inputBuffer = inputBuffer ?? throw new ArgumentNullException(nameof(inputBuffer));
        }

        private void Awake()
        {
            if (!ValidateSerializedReferences())
            {
                enabled = false;
            }
        }

        private void OnDisable()
        {
            ResetJoystick();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_activePointerId != NoPointer)
            {
                return;
            }

            _activePointerId = eventData.pointerId;
            UpdateJoystick(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (eventData.pointerId != _activePointerId)
            {
                return;
            }

            UpdateJoystick(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.pointerId != _activePointerId)
            {
                return;
            }

            ResetJoystick();
        }

        private void UpdateJoystick(PointerEventData eventData)
        {
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(_background, eventData.position,
                    eventData.pressEventCamera, out Vector2 localPosition))
            {
                return;
            }

            float halfWidth = _background.rect.width * 0.5f;
            float halfHeight = _background.rect.height * 0.5f;

            if (halfWidth <= 0f || halfHeight <= 0f)
            {
                ResetJoystick();
                return;
            }

            Vector2 normalizedPosition = new(localPosition.x / halfWidth, localPosition.y / halfHeight);

            normalizedPosition = Vector2.ClampMagnitude(normalizedPosition, 1f);

            if (normalizedPosition.sqrMagnitude <= JoystickDeadZone * JoystickDeadZone)
            {
                normalizedPosition = Vector2.zero;
            }

            _inputBuffer.SetMovementDirection(normalizedPosition);

            _handle.anchoredPosition = new Vector2(normalizedPosition.x * halfWidth, normalizedPosition.y * halfHeight);
        }

        private void ResetJoystick()
        {
            _activePointerId = NoPointer;

            if (_handle != null)
            {
                _handle.anchoredPosition = Vector2.zero;
            }

            _inputBuffer?.SetMovementDirection(Vector2.zero);
        }

        private bool ValidateSerializedReferences()
        {
            bool isValid = true;

            if (_background == null)
            {
                Debug.LogError("Mobile joystick background reference is missing", this);
                isValid = false;
            }

            if (_handle == null)
            {
                Debug.LogError("Mobile joystick handle reference is missing", this);
                isValid = false;
            }

            return isValid;
        }
    }
}
