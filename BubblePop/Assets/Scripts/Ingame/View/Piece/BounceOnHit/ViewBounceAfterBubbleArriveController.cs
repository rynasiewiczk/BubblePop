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

        private readonly List<IPiece> _aroundList = new List<IPiece>();

        public ViewBounceAfterBubbleArriveController(IGameStateController gameStateController, IFindingCellToShootPieceController findingCellToShootPieceController,
            SignalBus signalBus, IGridMap gridMap)
        {
            gameStateController.GamePlayState.Where(x => x == GamePlayState.PlacingPieceOnGrid).Subscribe(x =>
            {
                var arrivePosition = findingCellToShootPieceController.PieceDestinationPosition;
                var targetPosition = gridMap.GetViewPosition(arrivePosition);

                _aroundList.Clear();
                var bubblesAround = gridMap.GetPiecesAroundPosition(arrivePosition, _aroundList);

                foreach (var bubble in bubblesAround)
                {
                    _onBubbleHitSignal.Piece = bubble;
                    _onBubbleHitSignal.SoucrePosition = targetPosition;

                    signalBus.Fire(_onBubbleHitSignal);
                }
            });
        }
    }
}