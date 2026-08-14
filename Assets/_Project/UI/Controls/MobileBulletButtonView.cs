using System;
using Game.Core.Input;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Game.UI.Controls
{
    public sealed class MobileBulletButtonView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private const int NoPointer = int.MinValue;

        private MobileInputBuffer _inputBuffer;
        private int _activePointerId = NoPointer;

        [Inject]
        private void Construct(MobileInputBuffer inputBuffer)
        {
            _inputBuffer = inputBuffer ?? throw new ArgumentNullException(nameof(inputBuffer));
        }

        private void OnDisable()
        {
            Release();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_activePointerId != NoPointer)
            {
                return;
            }

            _activePointerId = eventData.pointerId;
            _inputBuffer.SetFireBulletHeld(true);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.pointerId != _activePointerId)
            {
                return;
            }

            Release();
        }

        private void Release()
        {
            _activePointerId = NoPointer;
            _inputBuffer?.SetFireBulletHeld(false);
        }
    }
}
