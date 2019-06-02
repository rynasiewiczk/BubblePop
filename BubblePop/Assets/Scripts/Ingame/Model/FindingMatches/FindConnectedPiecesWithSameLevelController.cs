using System.Collections.Generic;
using Enums;
using Project.Pieces;
using Project.Grid;
using UniRx;
using UnityEngine;

namespace Model.FindingMatches
{
    public class FindConnectedPiecesWithSameLevelController : IFindConnectedPiecesWithSameLevelController
    {
        private readonly IGridMap _gridMap = null;
        private readonly IGameStateController _gameStateController = null;
        public ReactiveCommand<List<IPiece>> CombinePieces { get; private set; } = new ReactiveCommand<List<IPiece>>();

        private List<IPiece> _bubblesBufferList = new List<IPiece>(15);

        public FindConnectedPiecesWithSameLevelController(IGridMap gridMap, IGameStateController gameStateController, IBubblesSpawner bubblesSpawner)
        {
            _gridMap = gridMap;
            _gameStateController = gameStateController;

            gameStateController.GamePlayState.Where(x => x == GamePlayState.PiecesCombining)
                .Subscribe(x => CombineBubbles(bubblesSpawner.LatestSpawnedBubble.Value));
        }

        private void CombineBubbles(IPiece piece)
        {
            _bubblesBufferList.Clear();
            _bubblesBufferList = _gridMap.FindPiecesToCollapse(piece, _bubblesBufferList);

            if (_bubblesBufferList.Count == 1)
            {
                _gameStateController.ChangeGamePlayState(GamePlayState.ScrollRows);
            }
            else if (_bubblesBufferList.Count > 1)
            {
                CombinePieces.Execute(_bubblesBufferList);
            }
            else
            {
                Debug.LogError("This should never happen.");
            }
        }
    }
}