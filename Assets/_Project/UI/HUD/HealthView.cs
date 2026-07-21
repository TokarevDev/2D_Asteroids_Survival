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
            if (_healthText == null)
            {
                Debug.LogError("Health text reference is missing", this);
                enabled = false;
                return;
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
    }
}
