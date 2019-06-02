using System;
using DG.Tweening;
using Project.Scripts.Popups.AnimationTransitions;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PopupTestAnimationTransition : PopupAnimationTransition
{
    [SerializeField] private Image _background = null;

    [Space] [SerializeField] private Vector3 _startScale = new Vector3(.75f, .75f, .75f);

    [FormerlySerializedAs("_transitionDuration")] [SerializeField]
    private float _backgroundTransitionDuration = .15f;

    [SerializeField] private float _scaleTransitionDuration = .15f;
    [SerializeField] private AnimationCurve _easing = null;

    public override void AnimateShow(Transform target, Action onFinished)
    {
        target.localScale = _startScale;
        target.DOScale(1, _scaleTransitionDuration).SetEase(_easing);

        if (_background != null)
        {
            AnimateBackground();
        }

        DOVirtual.DelayedCall(Mathf.Max(_backgroundTransitionDuration, _scaleTransitionDuration), () => { onFinished?.Invoke(); });
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
        _background.DOFade(.5f, _backgroundTransitionDuration);
    }
}