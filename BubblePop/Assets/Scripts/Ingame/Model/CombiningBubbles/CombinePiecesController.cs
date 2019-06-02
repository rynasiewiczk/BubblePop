using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Enums;
using Model.FindingMatches;
using Project.Pieces;
using Project.Grid;
using UniRx;
using UnityEngine;
using View.GridScoresDisplay;
using Zenject;

namespace Model.CombiningBubbles
{
    public class CombinePiecesController : ICombinePiecesController
    {
        private readonly IGridMap _gridMap = null;
        private readonly List<IPiece> _piecesToCollapseBufferList = new List<IPiece>(10);
        private readonly CombinePieceSignal _combinePieceSignal = new CombinePieceSignal();
        private readonly SpawnPieceOnGridSignal _spawnPieceOnGridSignal = new SpawnPieceOnGridSignal();
        private readonly SignalBus _signalBus = null;
        private readonly IGameStateController _gameStateController = null;
        public int LastCombinedPieceNeighboursWithSameLevelAmount { get; private set; }
        public ReactiveProperty<Vector2Int> PositionOfCollapse { get; private set; } = new ReactiveProperty<Vector2Int>();

        private readonly PiecesCombiningDoneSignal _piecesCombiningDoneSignal = new PiecesCombiningDoneSignal();

        private readonly float _piecesCombineDuration;

        public CombinePiecesController(IGridMap gridMap, IFindConnectedPiecesWithSameLevelController piecesWithSameLevelController, PiecesData piecesData,
            IGameStateController gameStateController, SignalBus signalBus)
        {
            _gridMap = gridMap;
            _piecesCombineDuration = piecesData.CombiningDuration;
            _gameStateController = gameStateController;
            _signalBus = signalBus;
            piecesWithSameLevelController.CombinePieces.Where(x => x != null && x.Count > 1).Subscribe(CollapsePieces);
        }

        private void CollapsePieces(List<IPiece> pieces)
        {
            _gameStateController.ChangeGamePlayState(GamePlayState.WaitingForPiecesCombine);

            var level = pieces[0].Level.Value;
            var levelAfterCombination = GetLevelAfterPiecesCombination(level, pieces.Count);

            var pieceWithMaxNeighboursWithResultLevel = FindPieceToCollapseTo(pieces, levelAfterCombination, out var toCollapseNeighboursAfterThisCollapse);
            LastCombinedPieceNeighboursWithSameLevelAmount = toCollapseNeighboursAfterThisCollapse;

            PositionOfCollapse.Value = pieceWithMaxNeighboursWithResultLevel.Position.Value;

            var collapseDuration = _piecesCombineDuration;

            foreach (var piece in pieces)
            {
                _combinePieceSignal.Piece = piece;
                var combinePosition = pieceWithMaxNeighboursWithResultLevel.Position.Value;
                _combinePieceSignal.CombinePosition = combinePosition;

                _signalBus.Fire(_combinePieceSignal);
            }

            DOVirtual.DelayedCall(collapseDuration, () =>
            {
                foreach (var piece in pieces)
                {
                    if (_gridMap.IsPiecePlayable(piece))
                    {
                        piece.Destroy();
                    }
                }

                _spawnPieceOnGridSignal.Level = levelAfterCombination;
                _spawnPieceOnGridSignal.Position = PositionOfCollapse.Value;

                _signalBus.Fire(_spawnPieceOnGridSignal);

                _piecesCombiningDoneSignal.ResultLevel = levelAfterCombination;
                _piecesCombiningDoneSignal.Position = PositionOfCollapse.Value;
                _signalBus.Fire(_piecesCombiningDoneSignal);

                _gameStateController.ChangeGamePlayState(GamePlayState.DropPiecesAfterCombining);
            }, false);
        }

        private IPiece FindPieceToCollapseTo(List<IPiece> pieces, int level, out int nextNeighbours)
        {
            var maxNumberOfConnections = 1;
            IPiece pieceToCollapseTo = null;

            foreach (var piece in pieces)
            {
                _piecesToCollapseBufferList.Clear();
                var piecesToCollect = _gridMap.FindPiecesToCollapse(level, piece.Position.Value, _piecesToCollapseBufferList);
                var piecesToCollapseCount = piecesToCollect.Count;
                if (!ShouldUpdateTgeBestFittingPiece(piecesToCollapseCount, maxNumberOfConnections, piece, pieceToCollapseTo))
                {
                    continue;
                }

                maxNumberOfConnections = piecesToCollapseCount;
                pieceToCollapseTo = piece;
            }

            if (pieceToCollapseTo == null)
            {
                var pieceAtTop = pieces.Aggregate((x1, x2) => x1.Position.Value.y > x2.Position.Value.y ? x1 : x2);
                pieceToCollapseTo = pieceAtTop;
            }

            nextNeighbours = maxNumberOfConnections;
            return pieceToCollapseTo;
        }

        private static bool ShouldUpdateTgeBestFittingPiece(int numberOfConnectionsAfterCollapse, int maxNumberOfConnections, IPiece piece,
            IPiece pieceToCollapseTo)
        {
            return numberOfConnectionsAfterCollapse > maxNumberOfConnections
                   || (numberOfConnectionsAfterCollapse == maxNumberOfConnections &&
                       (pieceToCollapseTo == null || piece.Position.Value.y > pieceToCollapseTo.Position.Value.y));
        }

        private int GetLevelAfterPiecesCombination(int level, int piecesCount)
        {
            var result = level + (piecesCount - 1);
            return result;
        }
    }
}