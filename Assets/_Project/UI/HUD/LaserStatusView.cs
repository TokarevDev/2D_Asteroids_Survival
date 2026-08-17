using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace Game.UI
{
    public sealed class LaserStatusView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _laserStatusText;

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
            if (_viewModel == null || _laserStatusText == null)
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

        private void OnLaserStatusChanged(int currentCharges, int maxCharges, float rechargeRemainingSeconds)
        {
            _laserStatusText.SetText("Laser: {0}/{1}\nRecharge: {2:0.0} s", currentCharges, maxCharges,
                rechargeRemainingSeconds);
        }

        private bool ValidateSerializedReferences()
        {
            if (_laserStatusText != null)
            {
                return true;
            }

            Debug.LogError("Laser status text reference is missing", this);
            return false;
        }
    }
}
