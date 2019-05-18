using DG.Tweening;
using Model.CombiningBubbles;
using Project.Grid;
using UnityEngine;
using Zenject;

namespace View
{
    public class BubbleViewCollapseComponent : MonoBehaviour
    {
        [SerializeField] private BubbleView _view;

        [Inject] private IGridMap _gridMap = null;
        [Inject] private SignalBus _signalBus = null;
        [Inject] private BubbleData _bubbleData = null;

        private void Awake()
        {
            Debug.Assert(_view, "Missing reference: _view", this);
        }

        private void Start()
        {
            _signalBus.Subscribe<CombineBubbleSignal>(Combine);
        }

        private void Combine(CombineBubbleSignal signal)
        {
            if (signal.Bubble != _view.Model)
            {
                return;
            }

            var viewCombinePosition = _gridMap.GetGridViewPosition(signal.CombinePosition);
            transform.DOMove(viewCombinePosition, _bubbleData.CombiningDuration);
        }
    }
}