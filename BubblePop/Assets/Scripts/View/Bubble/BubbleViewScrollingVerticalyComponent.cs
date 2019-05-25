using DG.Tweening;
using Model.ScrollingRowsDown;
using Project.Grid;
using UnityEngine;
using Zenject;

namespace View
{
    public class BubbleViewScrollingVerticalyComponent : MonoBehaviour
    {
        [SerializeField] private BubbleView _view = null;

        [Inject] private readonly SignalBus _signalBus = null;
        [Inject] private readonly GridSettings _gridSettings = null;

        private Tween _tween;

        private void Awake()
        {
            Debug.Assert(_view, "Missing reference: _view", this);
        }

        private void Start()
        {
            _signalBus.Subscribe<ScrollRowsSignal>(Scroll);
        }

        private void OnDestroy()
        {
            _signalBus.TryUnsubscribe<ScrollRowsSignal>(Scroll);
        }

        private void Scroll(ScrollRowsSignal signal)
        {
            var rowsToScroll = signal.RowsToScroll;
            var distanceToScroll = GridMapHelper.GetHeightOfRows(rowsToScroll);

            _tween?.Kill();

            _tween = transform.DOMoveY(transform.position.y + distanceToScroll, _gridSettings.ScrollOneRowDuration * Mathf.Abs(rowsToScroll));
        }
    }
}