using Game.Core.Input;
using UnityEngine;
using Zenject;

namespace Game.UI.Controls
{
    public sealed class MobileControlsVisibilityView : MonoBehaviour
    {
        [SerializeField] private GameObject _controlsRoot;

        private MobileInputBuffer _inputBuffer;

        [Inject]
        private void Construct(MobileInputBuffer inputBuffer)
        {
            _inputBuffer = inputBuffer;
        }

        private void Awake()
        {
            if (_controlsRoot == null)
            {
                Debug.LogError("Mobile controls root reference is missing", this);
                enabled = false;
                return;
            }

            _controlsRoot.SetActive(Application.isMobilePlatform);
        }

        private void OnDisable()
        {
            _inputBuffer?.Reset();
        }
    }
}
