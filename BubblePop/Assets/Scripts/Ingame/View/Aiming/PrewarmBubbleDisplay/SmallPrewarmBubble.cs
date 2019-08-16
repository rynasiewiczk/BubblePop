using DG.Tweening;
using UnityEngine;
using Zenject;

namespace View.Aiming.PrewarmBubbleDisplay
{
    public class SmallPrewarmBubble : MonoBehaviour
    {
        [Inject] private readonly BubbleViewSettings _bubbleViewSettings = null;

        [SerializeField] private PieceView _pieceView = null;

        public Vector2 SpawnPosition => transform.position;

        private Tween _tween = null;

        private void Awake()
        {
            Debug.Assert(_pieceView, "Missing reference: _pieceView", this);
        }

        public void Show(int level, bool instant)
        {
            _pieceView.Setup(level);

            var endScale = _bubbleViewSettings.SmallPrewarmBubbleSize;
            var duration = _bubbleViewSettings.SmallPrewarmBubbleAppearDuration;

            _tween?.Kill();

            if (!instant)
            {
                transform.localScale = Vector3.zero;
                _tween = transform.DOScale(endScale, duration);
            }
            else
            {
                transform.localScale = new Vector2(endScale, endScale);
            }
        }
    }
}