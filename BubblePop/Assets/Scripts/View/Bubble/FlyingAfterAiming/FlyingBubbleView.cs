using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

namespace View.FlyingAfterAiming
{
    public class FlyingBubbleView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer = null;
        [SerializeField] private TextMeshPro _text = null;

        [Inject] private readonly FlyingBubbleViewPool _flyingBubbleViewPool = null;

        private Vector2[] _path;
        private int _value;

        private void Awake()
        {
            Debug.Assert(_spriteRenderer, "Missing reference: _spriteRenderer", this);
            Debug.Assert(_text, "Missing reference: _text", this);
        }

        private void Start()
        {
            const int flyingBubbleTextSortingLayer = 998;
            _text.sortingOrder = flyingBubbleTextSortingLayer;
        }

        public void Setup(Vector3[] path, string value, Color color, float duration)
        {
            _spriteRenderer.color = color;
            _text.text = value;

            transform.DOPath(path, duration).OnComplete(() => { _flyingBubbleViewPool.Despawn(this); });
        }
    }
}