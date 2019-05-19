using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace View
{
    public class BubbleViewLayering : MonoBehaviour
    {
        [SerializeField] private BubbleView _view;

        [SerializeField] private SpriteRenderer _spriteRenderer = null;
        [SerializeField] private TextMeshPro _text = null;

        private int _latestRecordedRow = -1;

        private void Awake()
        {
            Debug.Assert(_view, "Missing reference: _view", this);
            Debug.Assert(_spriteRenderer, "Missing reference: _spriteRenderer", this);
            Debug.Assert(_text, "Missing reference: _text", this);
        }

        private void Update()
        {
            var model = _view.Model;
            if (model == null)
            {
                return;
            }

            if (_latestRecordedRow != model.Position.Value.y)
            {
                _latestRecordedRow = model.Position.Value.y;
                UpdateLayerOfRenderers(model.Position.Value.y);
            }
        }

        private void OnDisable()
        {
            _latestRecordedRow = -1;
        }

        private void UpdateLayerOfRenderers(int row)
        {
            _spriteRenderer.sortingOrder = row * BubbleViewSettings.RENDER_LAYERS_MULTIPLAYER;
            DOVirtual.DelayedCall(0,
                () => { _text.sortingOrder = row * BubbleViewSettings.RENDER_LAYERS_MULTIPLAYER + BubbleViewSettings.TEXT_RENDER_LAYER_ADDITION; });
        }
    }
}