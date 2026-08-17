using System;
using UnityEngine;
using Zenject;

namespace Game.UI.HUD
{
    public sealed class HealthView : MonoBehaviour
    {
        [SerializeField] private IconCounterView _healthCounter;

        private HealthViewModel _viewModel;

        [Inject]
        private void Construct(HealthViewModel viewModel)
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
            if (_viewModel == null || _healthCounter == null)
            {
                return;
            }

            _viewModel.HealthChanged += OnHealthChanged;

            if (_viewModel.IsInitialized)
            {
                OnHealthChanged(_viewModel.CurrentHealth, _viewModel.MaxHealth);
            }
        }

        private void OnDisable()
        {
            if (_viewModel == null)
            {
                return;
            }

            _viewModel.HealthChanged -= OnHealthChanged;
        }

        private void OnHealthChanged(int currentHealth, int maxHealth)
        {
            if (_healthCounter.Capacity != maxHealth)
            {
                Debug.LogError(
                    $"Health icon capacity {_healthCounter.Capacity} " +
                    $"does not match max health {maxHealth}", this);
                enabled = false;
                return;
            }

            _healthCounter.SetVisibleCount(currentHealth);
        }

        private bool ValidateSerializedReferences()
        {
            if (_healthCounter != null)
            {
                return true;
            }

            Debug.LogError("Health icon counter reference is missing", this);
            return false;
        }
    }
}
