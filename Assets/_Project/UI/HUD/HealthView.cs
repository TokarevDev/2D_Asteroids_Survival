using TMPro;
using UnityEngine;
using Zenject;

namespace Game.UI
{
    public sealed class HealthView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _healthText;

        private HealthViewModel _viewModel;

        [Inject]
        private void Construct(HealthViewModel viewModel)
        {
            _viewModel = viewModel;
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
            if (_viewModel == null || _healthText == null)
            {
                return;
            }

            _viewModel.HealthChanged += OnHealthChanged;

            OnHealthChanged(_viewModel.CurrentHealth, _viewModel.MaxHealth);
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
            _healthText.SetText("HP: {0}/{1}", currentHealth, maxHealth);
        }

        private bool ValidateSerializedReferences()
        {
            if (_healthText != null)
            {
                return true;
            }

            Debug.LogError("Health text reference is missing", this);
            return false;
        }
    }
}
