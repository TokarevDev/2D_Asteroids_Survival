using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace Game.UI
{
    public sealed class LaserStatusView : MonoBehaviour
    {
        [SerializeField] private IconCounterView _chargeCounter;
        [SerializeField] private TMP_Text _rechargeText;

        private LaserStatusViewModel _viewModel;

        [Inject]
        private void Construct(LaserStatusViewModel viewModel)
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
            if (_viewModel == null || _rechargeText == null || _chargeCounter == null)
            {
                return;
            }

            _viewModel.LaserStatusChanged += OnLaserStatusChanged;

            OnLaserStatusChanged(_viewModel.CurrentCharges, _viewModel.MaxCharges, _viewModel.RechargeRemainingSeconds);
        }

        private void OnDisable()
        {
            if (_viewModel == null)
            {
                return;
            }

            _viewModel.LaserStatusChanged -= OnLaserStatusChanged;
        }

        private void OnLaserStatusChanged(
            int currentCharges,
            int maxCharges,
            float rechargeRemainingSeconds)
        {
            if (_chargeCounter.Capacity != maxCharges)
            {
                Debug.LogError(
                    $"Laser charge icon capacity {_chargeCounter.Capacity} " +
                    $"does not match max charges {maxCharges}", this);
                enabled = false;
                return;
            }

            _chargeCounter.SetVisibleCount(currentCharges);
            _rechargeText.SetText(
                "Recharge: {0:0.0} s",
                rechargeRemainingSeconds);
        }

        private bool ValidateSerializedReferences()
        {
            if (_chargeCounter == null)
            {
                Debug.LogError("Laser charge counter reference is missing", this);
                return false;
            }

            if (_rechargeText == null)
            {
                Debug.LogError("Laser recharge text reference is missing", this);
                return false;
            }

            return true;
        }
    }
}
