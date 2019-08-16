using DG.Tweening;
using Project.Aiming;
using TMPro;
using UnityEngine;
using Zenject;

namespace View.Aiming.PrewarmBubbleDisplay
{
    public class AimingBubble : MonoBehaviour
    {
        [Inject] private readonly BubbleViewSettings _bubbleViewSettings = null;
        [Inject] private readonly IAimingStartPointProvider _aimingStartPointProvider = null;

        [SerializeField] private SmallPrewarmBubble _smallPrewarmBubble = null;
        [SerializeField] private PieceView _pieceView = null;
        
        
        private Tween _scalingTween = null;
        private Tween _positioningTween = null;

        private void Awake()
        {
            Debug.Assert(_pieceView, "Missing reference: _pieceView", this);
            Debug.Assert(_smallPrewarmBubble, "Missing reference: _smallPrewarmBubble", this);
        }

        public void Show(int level, bool instant)
        {
            gameObject.SetActive(true);
            _pieceView.Setup(level);

            var startScale = _bubbleViewSettings.SmallPrewarmBubbleSize;
            var targetPosition = _aimingStartPointProvider.GetAimingStartPoint();
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

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}