using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace Game.UI
{
    public sealed class ScoreView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreText;

        private ScoreViewModel _viewModel;

        [Inject]
        private void Construct(ScoreViewModel viewModel)
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
            if (_viewModel == null || _scoreText == null)
            {
                return;
            }

            _viewModel.ScoreChanged += OnScoreChanged;
            OnScoreChanged(_viewModel.Score);
        }

        private void OnDisable()
        {
            if (_viewModel == null)
            {
                return;
            }

            _viewModel.ScoreChanged -= OnScoreChanged;
        }

        private void OnScoreChanged(int score)
        {
            _scoreText.SetText("Score: {0}", score);
        }

        private bool ValidateSerializedReferences()
        {
            if (_scoreText != null)
            {
                return true;
            }

            Debug.LogError("Score text reference is missing", this);
            return false;
        }
    }
}
