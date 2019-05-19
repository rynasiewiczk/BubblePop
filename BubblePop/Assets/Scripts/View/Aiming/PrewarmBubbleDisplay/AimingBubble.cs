using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

namespace View.Aiming.PrewarmBubbleDisplay
{
    public class AimingBubble : MonoBehaviour
    {
        [Inject] private readonly BubbleViewSettings _bubbleViewSettings = null;
        [Inject] private readonly AimingSettings _aimingSettings = null;
        [Inject] private readonly Camera _camera = null;

        [SerializeField] private SpriteRenderer _spriteRenderer = null;
        [SerializeField] private TextMeshPro _text = null;
        [SerializeField] private SmallPrewarmBubble _smallPrewarmBubble = null;

        private Tween _scalingTween = null;
        private Tween _positioningTween = null;

        private void Awake()
        {
            Debug.Assert(_spriteRenderer, "Missing reference: _spriteRenderer", this);
            Debug.Assert(_text, "Missing reference: _text", this);
            Debug.Assert(_smallPrewarmBubble, "Missing reference: _smallPrewarmBubble", this);
        }

        public void Show(Color color, int value, bool instant)
        {
            _spriteRenderer.color = color;
            _text.text = value.ToString();

            var startScale = _bubbleViewSettings.SmallPrewarmBubbleSize;
            Vector2 targetPosition = _camera.ViewportToWorldPoint(_aimingSettings.GetAimingPositionInViewPortPosition());
            var duration = _bubbleViewSettings.AimingBubbleTransitionDuration;

            _scalingTween?.Kill();
            _positioningTween?.Kill();
            if (!instant)
            {
                transform.localScale = new Vector3(startScale, startScale, 0);
                _scalingTween = transform.DOScale(_bubbleViewSettings.AimingBubbleOverscale, duration).OnComplete(() =>
                {
                    _scalingTween = transform.DOScale(1, _bubbleViewSettings.AimingBubbleFromOverscaleToNormalDuration);
                });

                transform.position = _smallPrewarmBubble.SpawnPosition;
                _positioningTween = transform.DOMove(targetPosition, duration);
            }
            else
            {
                transform.localScale = Vector3.one;
                transform.position = targetPosition;
            }
        }
    }
}