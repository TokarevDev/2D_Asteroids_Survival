using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace Game.UI.HUD
{
    public sealed class TimerView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _timerText;

        private TimerViewModel _viewModel;

        [Inject]
        private void Construct(TimerViewModel viewModel)
        {
            _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
        }

        private void Awake()
        {
            if (!ValidateSerializedReferences())
            {
                enabled = false;
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

        private bool ValidateSerializedReferences()
        {
            if (_timerText != null)
            {
                return true;
            }

            Debug.LogError("Timer text reference is missing", this);
            return false;
        }
    }
}
