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
        private readonly List<IPiece> _bubblesToCollapseBufferList = new List<IPiece>(10);
        private readonly CombineBubbleSignal _combineBubbleSignal = new CombineBubbleSignal();
        private readonly SpawnPieceOnGridSignal _spawnPieceOnGridSignal = new SpawnPieceOnGridSignal();
        private readonly SignalBus _signalBus = null;
        private readonly IGameStateController _gameStateController = null;
        public int LastCombinedBubbleNeighboursWithSameLevelAmount { get; private set; }
        public ReactiveProperty<Vector2Int> PositionOfCollapse { get; private set; } = new ReactiveProperty<Vector2Int>();

        private readonly BubblesCombiningDoneSignal _bubblesCombiningDoneSignal = new BubblesCombiningDoneSignal();

        private readonly float _bubblesCombineDuration;

        public CombinePiecesController(IGridMap gridMap, IFindConnectedPiecesWithSameLevelController piecesWithSameLevelController, PiecesData piecesData,
            IGameStateController gameStateController, SignalBus signalBus)
        {
            _gridMap = gridMap;
            _bubblesCombineDuration = piecesData.CombiningDuration;
            _gameStateController = gameStateController;
            _signalBus = signalBus;
            piecesWithSameLevelController.CombinePieces.Where(x => x != null && x.Count > 1).Subscribe(CollapseBubbles);
        }

        private void CollapseBubbles(List<IPiece> bubbles)
        {
            _gameStateController.ChangeGamePlayState(GamePlayState.WaitingForBubblesCombine);

            var level = bubbles[0].Level.Value;
            var levelAfterCombination = GetLevelAfterBubblesCombination(level, bubbles.Count);

            var bubbleWithMaxNeighboursWithResultLevel = FindBubbleToCollapseTo(bubbles, levelAfterCombination, out var toCollapseNeighboursAfterThisCollapse);
            LastCombinedBubbleNeighboursWithSameLevelAmount = toCollapseNeighboursAfterThisCollapse;

            PositionOfCollapse.Value = bubbleWithMaxNeighboursWithResultLevel.Position.Value;

            var collapseDuration = _bubblesCombineDuration;

            foreach (var bubble in bubbles)
            {
                _combineBubbleSignal.Piece = bubble;
                var combinePosition = bubbleWithMaxNeighboursWithResultLevel.Position.Value;
                _combineBubbleSignal.CombinePosition = combinePosition;

                _signalBus.Fire(_combineBubbleSignal);
            }

            DOVirtual.DelayedCall(collapseDuration, () =>
            {
                foreach (var bubble in bubbles)
                {
                    if (_gridMap.IsBubblePlayable(bubble))
                    {
                        bubble.Destroy();
                    }
                }

                _spawnPieceOnGridSignal.Level = levelAfterCombination;
                _spawnPieceOnGridSignal.Position = PositionOfCollapse.Value;

                _signalBus.Fire(_spawnPieceOnGridSignal);

                _bubblesCombiningDoneSignal.ResultLevel = levelAfterCombination;
                _bubblesCombiningDoneSignal.Position = PositionOfCollapse.Value;
                _signalBus.Fire(_bubblesCombiningDoneSignal);

                _gameStateController.ChangeGamePlayState(GamePlayState.DropBubblesAfterCombining);
            }, false);
        }

        private IPiece FindBubbleToCollapseTo(List<IPiece> bubbles, int level, out int nextNeighbours)
        {
            var maxNumberOfConnections = 1;
            IPiece pieceToCollapseTo = null;

            foreach (var bubble in bubbles)
            {
                _bubblesToCollapseBufferList.Clear();
                var bubblesToCollect = _gridMap.FindBubblesToCollapse(level, bubble.Position.Value, _bubblesToCollapseBufferList);
                var bubblesToCollapseCount = bubblesToCollect.Count;
                if (!ShouldUpdateBetterFitBubble(bubblesToCollapseCount, maxNumberOfConnections, bubble, pieceToCollapseTo))
                {
                    continue;
                }

                maxNumberOfConnections = bubblesToCollapseCount;
                pieceToCollapseTo = bubble;
            }

            if (pieceToCollapseTo == null)
            {
                var bubbleAtTop = bubbles.Aggregate((x1, x2) => x1.Position.Value.y > x2.Position.Value.y ? x1 : x2);
                pieceToCollapseTo = bubbleAtTop;
            }

            nextNeighbours = maxNumberOfConnections;
            return pieceToCollapseTo;
        }

        private static bool ShouldUpdateBetterFitBubble(int numberOfConnectionsAfterCollapse, int maxNumberOfConnections, IPiece piece,
            IPiece pieceToCollapseTo)
        {
            return numberOfConnectionsAfterCollapse > maxNumberOfConnections
                   || (numberOfConnectionsAfterCollapse == maxNumberOfConnections &&
                       (pieceToCollapseTo == null || piece.Position.Value.y > pieceToCollapseTo.Position.Value.y));
        }

        private int GetLevelAfterBubblesCombination(int level, int bubblesCount)
        {
            var result = level + (bubblesCount - 1);
            return result;
        }
    }
}