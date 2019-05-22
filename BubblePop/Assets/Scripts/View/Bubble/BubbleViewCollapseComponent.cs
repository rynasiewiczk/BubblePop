using DG.Tweening;
using Model.CombiningBubbles;
using Project.Grid;
using UnityEngine;
using Zenject;

namespace View
{
    public class BubbleViewCollapseComponent : MonoBehaviour
    {
        [SerializeField] private BubbleView _view = null;

        [Inject] private readonly IGridMap _gridMap = null;
        [Inject] private readonly SignalBus _signalBus = null;
        [Inject] private readonly PiecesData _piecesData = null;

        private void Awake()
        {
            Debug.Assert(_view, "Missing reference: _view", this);
        }

        private void Start()
        {
            _signalBus.Subscribe<CombineBubbleSignal>(Combine);
        }

        private void OnDestroy()
        {
            _signalBus.TryUnsubscribe<CombineBubbleSignal>(Combine);
        }

        private void Combine(CombineBubbleSignal signal)
        {
            if (signal.Bubble != _view.Model)
            {
                return;
            }

            var viewCombinePosition = _gridMap.GetCellsViewPosition(signal.CombinePosition);
            transform.DOMove(viewCombinePosition, _piecesData.CombiningDuration);
        }
    }
}