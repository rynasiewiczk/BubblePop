using DG.Tweening;
using Model.ScrollingRowsDown;
using Project.Grid;
using UnityEngine;
using Zenject;
using UniRx;

namespace View
{
    public class BubbleViewScrollingVerticalyComponent : MonoBehaviour
    {
        [SerializeField] private GridBubble _view = null;

        [Inject] private readonly IScrollRowsController _scrollRowsController = null;
        [Inject] private readonly GridSettings _gridSettings = null;

        private Tween _tween;

        private void Awake()
        {
            Debug.Assert(_view, "Missing reference: _view", this);
        }

        private void Start()
        {
            _scrollRowsController.RowsScrolled.Subscribe(Scroll);
        }

        private void Scroll(int rowsToScroll)
        {
            var distanceToScroll = GridMapHelper.GetHeightOfRowsInWorldPosition(rowsToScroll);
            _tween?.Kill();
            _tween = transform.DOMoveY(transform.position.y + distanceToScroll, _gridSettings.ScrollOneRowDuration * Mathf.Abs(rowsToScroll))
                .SetEase(_gridSettings.ScrollAnimationCurve);
        }
    }
}