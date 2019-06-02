using System;
using DG.Tweening;
using UnityEngine;
using Zenject;
using UniRx;

namespace OutGame
{
    public class MainMenuPanelController : MonoBehaviour
    {
        private RectTransform _rectTransform = null;
        [Inject] private readonly IIngameSceneVisibilityController _ingameSceneVisibilityController = null;

        private Tween _tween = null;

        private void Start()
        {
            _rectTransform = transform as RectTransform;

            _ingameSceneVisibilityController.OnIngamePaused.Subscribe(x => Show());
            _ingameSceneVisibilityController.OnIngameUnpaused.Subscribe(x => Hide());
        }

        private void Show()
        {
            _tween?.Kill();
            _tween = _rectTransform.DOAnchorPos(Vector2.zero, .45f);
        }

        private void Hide()
        {
            _tween?.Kill();

            _tween = DOVirtual.DelayedCall(0, () => { _tween = _rectTransform.DOAnchorPos(_rectTransform.rect.width * 1.1f * Vector2.right, .45f); });
        }
    }
}