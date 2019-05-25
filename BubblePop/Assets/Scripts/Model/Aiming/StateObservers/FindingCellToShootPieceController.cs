using System.Linq;
using Enums;
using Model;
using Project.Grid;
using Project.Input;
using UniRx;
using UnityEngine;

namespace Project.Aiming
{
    public class FindingCellToShootPieceController : IFindingCellToShootPieceController
    {
        private readonly IAimEndPointFinder _aimEndPointFinder = null;
        private readonly IGameStateController _gameStateController = null;
        private readonly IGridMap _gridMap = null;

        public ReactiveProperty<Vector2[]> PieceFlyPath { get; }

        public Vector2Int PieceDestinationPosition
        {
            get
            {
                var lastPathPosition = PieceFlyPath.Value.Last();
                return new Vector2Int((int) lastPathPosition.x, (int) lastPathPosition.y);
            }
        }

        public FindingCellToShootPieceController(IInputEventsNotifier inputEventsNotifier, IAimEndPointFinder aimEndPointFinder,
            IGameStateController gameStateController, IGridMap gridMap)
        {
            PieceFlyPath = new ReactiveProperty<Vector2[]>();

            _aimEndPointFinder = aimEndPointFinder;
            _gameStateController = gameStateController;
            _gridMap = gridMap;

            inputEventsNotifier.OnInputEnd.Where(x => _gameStateController.GamePlayState.Value == GamePlayState.Aiming)
                .Subscribe(x => GetPlaceToSpawnPiece());
        }

        private void GetPlaceToSpawnPiece()
        {
            if (_aimEndPointFinder.AimedPieceData != null)
            {
                var piecesViewPosition = _gridMap.GetCellsViewPosition(_aimEndPointFinder.AimedPieceData.Piece.Position.Value);
                var hitPointRelativeToBubblesPosition = _aimEndPointFinder.AimPath.Last() - piecesViewPosition;
                var positionToSpawnPiece = _gridMap.GetPositionToSpawnPiece(_aimEndPointFinder.AimedPieceData.Piece,
                    _aimEndPointFinder.AimedPieceData.AimedSide, hitPointRelativeToBubblesPosition);

                var cellToSpawnPiece = _gridMap.CellsRegistry.FirstOrDefault(x => x.Position == positionToSpawnPiece);
                if (cellToSpawnPiece == null)
                {
                    Debug.LogError("Trying to spawn piece outside of grid. Ignoring the try. Provided position: " + positionToSpawnPiece);
                    _gameStateController.ChangeGamePlayState(GamePlayState.Idle);
                }
                else if (_gridMap.PiecesRegistry.FirstOrDefault(x => x.Position.Value == positionToSpawnPiece) != null)
                {
                    Debug.LogError("Trying to spawn piece on cell that already has a piece. Ignoring the try. Provided position: " + positionToSpawnPiece);
                    _gameStateController.ChangeGamePlayState(GamePlayState.Idle);
                }
                else
                {
                    PieceFlyPath.Value = _aimEndPointFinder.AimedPieceData.PathFromAimingPosition;
                }
            }
            else
            {
                _gameStateController.ChangeGamePlayState(GamePlayState.Idle);
            }
        }
    }
}