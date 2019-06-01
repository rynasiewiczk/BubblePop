using DG.Tweening;
using Model.Progress.PlayerLevelController;
using TMPro;
using UnityEngine;
using Zenject;
using UniRx;
using UnityEngine.UI;

namespace View.UI
{
    public class PlayerLevelDisplayController : MonoBehaviour
    {
        [Inject] private readonly IPlayerLevelController _playerLevelController = null;

        [SerializeField] private TextMeshProUGUI _text = null;
        [SerializeField] private Image _innerPanel = null;

        [SerializeField] private float _innerPanelTweenSizeGrow = 1.1f;
        [SerializeField] private float _innerPanelGrowDuration = .2f;

        [SerializeField] private float _textTweenSizeGrow = 1.1f;
        [SerializeField] private float _textGrowDuration = .3f;

        private Tween _innerPanelTween = null;
        private Tween _textTween = null;

        private Vector2 _innerPanelStartSize;
        private Vector3 _textStartSize;

        private bool _firstRun = true;

        private void Awake()
        {
            Debug.Assert(_innerPanel, "Missing reference: _innerPanel", this);
            Debug.Assert(_text, "Missing reference: _text", this);
        }

        private void Start()
        {
            _text.text = _playerLevelController.PlayerLevel.Value.ToString();
            _playerLevelController.PlayerLevel.Skip(1).Subscribe(UpdateLevel);
        }
        
        private void UpdateLevel(int x)
        {
            if (_firstRun)
            {
                _innerPanelStartSize = _innerPanel.rectTransform.localScale;
                _textStartSize = _text.transform.localScale;

                _firstRun = false;
            }

            _text.text = x.ToString();

            _innerPanelTween?.Kill();
            _textTween?.Kill();

            _innerPanelTween = _innerPanel.rectTransform.DOScale(_innerPanelStartSize * _innerPanelTweenSizeGrow, _innerPanelGrowDuration / 2)
                .OnComplete(() =>
                {
                    _innerPanelTween = _innerPanel.rectTransform.DOScale(_innerPanelStartSize, _innerPanelGrowDuration / 2);
                    _textTween = _text.transform.DOScale(_textStartSize * _textTweenSizeGrow, _textGrowDuration / 2).OnComplete(() =>
                    {
                        _textTween = _text.transform.DOScale(_textStartSize, _textGrowDuration / 2);
                    });
                });
        }
    }
}