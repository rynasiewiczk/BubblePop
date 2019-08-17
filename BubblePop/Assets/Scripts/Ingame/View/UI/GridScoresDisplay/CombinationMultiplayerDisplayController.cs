using DG.Tweening;
using Enums;
using Model;
using Model.FindingMatches;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

public class CombinationMultiplayerDisplayController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text = null;
    [SerializeField] private float _startTextScale = .35f;
    [SerializeField] private float _scaleDuration = .4f;
    [SerializeField] private float _startAlpha = .35f;
    [SerializeField] private float _alphaDuration = .2f;

    [Inject] private readonly IGameStateController _gameStateController = null;
    [Inject] private readonly IFindConnectedPiecesWithSameLevelController _findConnectedPiecesWithSameLevelController = null;

    private Tween _scaleTween = null;
    private Tween _fadeTween = null;

    private void Start()
    {
        HideText();
        _gameStateController.GamePlayState.Where(x => x == GamePlayState.ScrollRows).Subscribe(x => SetCombinationMultiplayerText());
    }

    private void SetCombinationMultiplayerText()
    {
        var combinationsInRow = _findConnectedPiecesWithSameLevelController.CombinationsInRow.Value;
        if (combinationsInRow <= 1)
        {
            return;
        }

        HideText();
        ShowText(combinationsInRow);
    }

    private void HideText()
    {
        _scaleTween?.Kill();
        _fadeTween?.Kill();
        _text.gameObject.SetActive(false);
        _text.transform.localScale = Vector3.one * _startTextScale;
        _text.alpha = _startAlpha;
    }

    private void ShowText(int multiplier)
    {
        _text.text = $"x{multiplier.ToString()}";
        _text.gameObject.SetActive(true);

        _scaleTween?.Kill();
        _scaleTween = _text.transform.DOScale(1, _scaleDuration);

        _fadeTween?.Kill();
        _fadeTween = _text.DOFade(1, _alphaDuration).OnComplete(() => { _fadeTween = _text.DOFade(0, _alphaDuration); });
    }
}