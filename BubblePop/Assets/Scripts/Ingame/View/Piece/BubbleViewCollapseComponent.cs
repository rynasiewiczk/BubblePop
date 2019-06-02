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

            _signalBus.Subscribe<CombinePieceSignal>(Combine);
        }


        private void OnDestroy()
        {
            _signalBus.TryUnsubscribe<CombinePieceSignal>(Combine);
        }

        private void Combine(CombinePieceSignal signal)
        {
            if (signal.Piece != _view.Model)
            {
                return;
            }

            var viewCombinePosition = _gridMap.GetViewPosition(signal.CombinePosition);
            transform.DOMove(viewCombinePosition, _piecesData.CombiningDuration);
        }
    }
}