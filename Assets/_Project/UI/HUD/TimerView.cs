using TMPro;
using UnityEngine;
using Zenject;

namespace Game.UI
{
    public sealed class TimerView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _timerText;

        private TimerViewModel _viewModel;

        [Inject]
        private void Construct(TimerViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        private void Awake()
        {
            if (_timerText == null)
            {
                Debug.LogError("Timer text reference is missing", this);
                enabled = false;
                return;
            }
        }

        private void OnEnable()
        {
            if (_viewModel == null || _timerText == null)
            {
                return;
            }

            _viewModel.TimeChanged += OnTimeChanged;

            OnTimeChanged(_viewModel.Minutes, _viewModel.Seconds);
        }

        private void OnDisable()
        {
            if (_viewModel == null)
            {
                return;
            }

            _viewModel.TimeChanged -= OnTimeChanged;
        }

        private void OnTimeChanged(int minutes, int seconds)
        {
            _timerText.SetText("{0:00}:{1:00}", minutes, seconds);
        }
    }
}
