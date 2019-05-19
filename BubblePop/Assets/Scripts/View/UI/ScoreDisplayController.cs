using DG.Tweening;
using Model.ScoreController;
using TMPro;
using UnityEngine;
using Zenject;
using UniRx;

public class ScoreDisplayController : MonoBehaviour
{
    [Inject] private readonly IScoreController _scoreController = null;

    [SerializeField] private TextMeshProUGUI _text = null;

    [SerializeField] private float _textTweenSizeGrow = 1.1f;
    [SerializeField] private float _textGrowDuration = .3f;

    private Tween _textTween = null;

    private Vector3 _textStartSize;

    private bool _firstRun = true;

    private void Awake()
    {
        Debug.Assert(_text, "Missing reference: _text", this);
    }

    private void Start()
    {
        _text.text = _scoreController.Score.Value.ToString();
        _scoreController.Score.Skip(1).Subscribe(UpdateScore);

        _text.text = 0.ToString();
    }

    private void UpdateScore(int x)
    {
        if (_firstRun)
        {
            _textStartSize = _text.transform.localScale;

            _firstRun = false;
        }

        _text.text = x.ToString();

        _textTween?.Kill();

        _textTween = _text.transform.DOScale(_textStartSize * _textTweenSizeGrow, _textGrowDuration / 2).OnComplete(() =>
        {
            _textTween = _text.transform.DOScale(_textStartSize, _textGrowDuration / 2);
        });
    }
}