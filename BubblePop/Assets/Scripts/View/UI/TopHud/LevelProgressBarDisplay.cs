using DG.Tweening;
using Model.Progress.PlayerLevelController;
using Model.ScoreController;
using TMPro;
using UnityEngine;
using Zenject;
using UniRx;
using UnityEngine.UI;

public class LevelProgressBarDisplay : MonoBehaviour
{
    [Inject] private readonly IScoreController _scoreController = null;
    [Inject] private readonly IPlayerLevelController _playerLevelController = null;
    [Inject] private readonly PlayerLevelSettings _playerLevelSettings = null;

    [Inject] private readonly PiecesData _piecesData = null;

    [SerializeField] private Image _colorfulPanel = null;

    [SerializeField] private Image _leftEdgeImage = null;
    [SerializeField] private Image _rightEdgeImage = null;

    [SerializeField] private TextMeshProUGUI _currentLevelText = null;
    [SerializeField] private TextMeshProUGUI _nextLevelText = null;

    [SerializeField] private float _fillDuration = .1f;
    public float FillDuration => _fillDuration;

    private Tween _tween = null;
    
    private void Awake()
    {
        Debug.Assert(_colorfulPanel, "Missing reference: _colorfulPanel", this);
        Debug.Assert(_leftEdgeImage, "Missing reference: _leftEdgeImage", this);
        Debug.Assert(_rightEdgeImage, "Missing reference: _rightEdgeImage", this);
        Debug.Assert(_currentLevelText, "Missing reference: _currentLevelText", this);
        Debug.Assert(_nextLevelText, "Missing reference: _nextLevelText", this);
    }

    private void Start()
    {
        _scoreController.Score.Subscribe(UpdateScoreDisplay);
    }

    private void UpdateScoreDisplay(int score)
    {
        var currentLevel = _playerLevelController.PlayerLevel.Value;
        var currentLevelColor = _piecesData.GetColorForLevel(currentLevel);
        var nextLevelColor = _piecesData.GetColorForLevel(currentLevel + 1);

        var normalizedProgress = _playerLevelSettings.GetCurrentLevelNormalizedProgress(score);
        
        _tween?.Kill();
        _tween = _colorfulPanel.DOFillAmount(normalizedProgress, _fillDuration);
        _colorfulPanel.color = currentLevelColor;

        _leftEdgeImage.color = currentLevelColor;
        _currentLevelText.text = currentLevel.ToString();

        _rightEdgeImage.color = nextLevelColor;
        _nextLevelText.text = (currentLevel + 1).ToString();
    }

    public Vector2 GetProgressBarRightEdge()
    {
        var leftEdgeXPos = _colorfulPanel.rectTransform.anchoredPosition.x - _colorfulPanel.rectTransform.rect.width / 2;
        var fillXPos = leftEdgeXPos + _colorfulPanel.fillAmount * _colorfulPanel.rectTransform.rect.width;
        return new Vector2(fillXPos, _colorfulPanel.rectTransform.anchoredPosition.y + _colorfulPanel.rectTransform.rect.height / 2);
    }
}