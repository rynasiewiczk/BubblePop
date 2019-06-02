using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

namespace View.Aiming.PrewarmBubbleDisplay
{
    public class SmallPrewarmBubble : MonoBehaviour
    {
        [Inject] private readonly BubbleViewSettings _bubbleViewSettings = null;

        [SerializeField] private SpriteRenderer _spriteRenderer = null;
        [SerializeField] private TextMeshPro _text = null;

        public Vector2 SpawnPosition => transform.position;

        private Tween _tween = null;

        private void Awake()
        {
            Debug.Assert(_spriteRenderer, "Missing reference: _spriteRenderer", this);
            Debug.Assert(_text, "Missing reference: _text", this);
        }

        public void Show(Color color, int value, bool instant)
        {
            _spriteRenderer.color = color;
            _text.text = value.ToString();


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