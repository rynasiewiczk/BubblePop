using System;
using DG.Tweening;
using Project.Grid;
using UnityEngine;
using Zenject;

namespace View
{
    public class BubbleViewAppearingComponent : MonoBehaviour
    {
        [SerializeField] private GridBubble _view = null;
        [Inject] private readonly GridSettings _gridSettings = null;
        [Inject] private readonly BubbleViewSettings _bubbleViewSettings = null;

        private bool _scaledUp = true;
        private Tween _tween;

        private GridBubble View
        {
            get
            {
                if (_view == null) _view = GetComponentInParent<GridBubble>();
                return _view;
            }
        }

        private void Awake()
        {
            Debug.Assert(_view, "Missing reference: _view", this);
        }

        private void Start()
        {
            View.OnSetuped += InitScale;
            ScaleDown(true);
            InitScale();
        }

        private void OnDestroy()
        {
            View.OnSetuped -= InitScale;
        }

        private void Update()
        {
            if (View == null)
            {
                ScaleDown(true);
                return;
            }

            if (View.Model == null)
            {
                ScaleDown(true);
                return;
            }

            ScaleIfNeeded(View.Model.Position.Value);
        }

        private void InitScale()
        {
            if (View == null || View.Model == null)
            {
                return;
            }

            if (View.Model.Position.Value.y >= _gridSettings.StartGridSize.y)
            {
                ScaleDown(true);
            }
            else
            {
                ScaleUp(true);
            }
        }

        private void ScaleIfNeeded(Vector2Int position)
        {
            if (position.y >= _gridSettings.StartGridSize.y && _scaledUp)
            {
                ScaleDown(false);
                return;
            }

            if (position.y < _gridSettings.StartGridSize.y && !_scaledUp)
            {
                ScaleUp(false);
            }
        }

        private void ScaleUp(bool snap)
        {
            _tween?.Kill();

            if (snap)
            {
                transform.localScale = Vector3.one;
            }
            else
            {
                _tween = transform.DOScale(Vector3.one, _bubbleViewSettings.AppearScaleDuration).SetEase(_bubbleViewSettings.AppearFromTopScaleEase);
            }

            _scaledUp = true;
        }

        private void ScaleDown(bool snap)
        {
            _tween?.Kill();

            if (snap)
            {
                transform.localScale = Vector3.zero;
            }
            else
            {
                _tween = transform.DOScale(Vector3.zero, _bubbleViewSettings.AppearScaleDuration);
            }

            _scaledUp = false;
        }
    }
}