using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace View.FlyingAfterAiming
{
    public class FlyingBubbleView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer = null;
        [SerializeField] private TextMeshPro _text = null;
        
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

        public void Setup(Vector3[] path, int value, Color color, float duration, Action onFlyFinish)
        {
            _spriteRenderer.color = color;
            _text.text = value.ToString();

            transform.DOPath(path, duration).OnComplete(() => onFlyFinish?.Invoke());
        }
    }
}