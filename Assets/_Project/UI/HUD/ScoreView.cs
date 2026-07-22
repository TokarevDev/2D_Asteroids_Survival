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
            _viewModel = viewModel;
        }

        private void Awake()
        {
            if (_scoreText == null)
            {
                Debug.LogError("Score text reference is missing", this);
                enabled = false;
                return;
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
    }
}
