using System;
using DG.Tweening;
using Project.Scripts.Popups.AnimationTransitions;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PopupGenericAnimationTransition : PopupAnimationTransition
{
    [Inject] private readonly PopupAnimationDefaultParameters _defaultParameters = null;

    [SerializeField] private Image _background = null;

    public override void AnimateShow(Transform target, Action onFinished)
    {
        target.localScale = Vector2.one * _defaultParameters.StartScale;
        target.DOScale(1, _defaultParameters.TransitionDuration).SetEase(_defaultParameters.Easing).OnComplete(() => onFinished());

        if (_background == null) return;
        AnimateBackground();
    }

    public override void AnimateHide(Transform target, Action onFinished)
    {
        onFinished();
    }

    private void AnimateBackground()
    {
        var bgColor = _background.color;
        bgColor.a = 0;
        _background.color = bgColor;
        _background.DOFade(.5f, _defaultParameters.TransitionDuration);
    }
}