using System;
using Game.Core.Input;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Game.UI.Controls
{
    public sealed class MobileLaserButtonView : MonoBehaviour, IPointerDownHandler
    {
        private MobileInputBuffer _inputBuffer;

        [Inject]
        private void Construct(MobileInputBuffer inputBuffer)
        {
            _inputBuffer = inputBuffer ?? throw new ArgumentNullException(nameof(inputBuffer));
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _inputBuffer.RequestLaserFire();
        }
    }
}
