using System.Collections.Generic;
using Enums;
using Model;
using Project.Aiming;
using Project.Pieces;
using Project.Grid;
using UniRx;
using Zenject;

namespace View
{
    public class ViewBounceAfterBubbleArriveController : IViewBounceAfterBubbleArriveController
    {
        private readonly OnBubbleHitSignal _onBubbleHitSignal = new OnBubbleHitSignal();

        private readonly List<IBubble> _aroundList = new List<IBubble>();

        public ViewBounceAfterBubbleArriveController(IGameStateController gameStateController, IFindingCellToShootPieceController findingCellToShootPieceController,
            SignalBus signalBus, IGridMap gridMap)
        {
            gameStateController.GamePlayState.Where(x => x == GamePlayState.PlacingBubbleOnGrid).Subscribe(x =>
            {
                var arrivePosition = findingCellToShootPieceController.PieceDestinationPosition;
                var targetPosition = gridMap.GetCellsViewPosition(arrivePosition);

                _aroundList.Clear();
                var bubblesAround = gridMap.GetBubblesAroundPosition(arrivePosition, _aroundList);

                foreach (var bubble in bubblesAround)
                {
                    _onBubbleHitSignal.Bubble = bubble;
                    _onBubbleHitSignal.SoucrePosition = targetPosition;

                    signalBus.Fire(_onBubbleHitSignal);
                }
            });
        }
    }
}