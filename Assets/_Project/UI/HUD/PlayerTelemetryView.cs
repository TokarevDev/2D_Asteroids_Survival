using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace Game.UI.HUD
{
    public sealed class PlayerTelemetryView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _telemetryText;

        private PlayerTelemetryViewModel _viewModel;

        [Inject]
        private void Construct(PlayerTelemetryViewModel viewModel)
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
            if (_viewModel == null || _telemetryText == null)
            {
                return;
            }

            _viewModel.TelemetryChanged += OnTelemetryChanged;

            OnTelemetryChanged(_viewModel.Position, _viewModel.RotationDegrees, _viewModel.Speed);
        }

        private void OnDisable()
        {
            if (_viewModel == null)
            {
                return;
            }

            _viewModel.TelemetryChanged -= OnTelemetryChanged;
        }

        private void OnTelemetryChanged(Vector2 position, float rotationDegrees, float speed)
        {
            _telemetryText.SetText("Position X: {0:0.0}  Y: {1:0.0}\nAngle: {2:0.0} deg\nSpeed: {3:0.0}", position.x,
                position.y, rotationDegrees, speed);
        }

        private bool ValidateSerializedReferences()
        {
            if (_telemetryText != null)
            {
                return true;
            }

            Debug.LogError("Player telemetry text reference is missing", this);
            return false;
        }
    }
}
