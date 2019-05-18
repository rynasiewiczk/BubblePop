using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Enums;
using Model.FindingMatches;
using Project.Bubbles;
using Project.Grid;
using UniRx;
using UnityEngine;
using Zenject;

namespace Model.CombiningBubbles
{
    public class CombineBubbles : ICombineBubbles
    {
        private readonly IGridMap _gridMap = null;
        private readonly List<IBubble> _bubblesToCollapseBufferList = new List<IBubble>(10);
        private BubbleData _bubbleData = null;
        private CombineBubbleSignal _combineBubbleSignal = new CombineBubbleSignal();
        private SignalBus _signalBus = null;
        private IGameStateController _gameStateController = null;

        public CombineBubbles(IGridMap gridMap, IFindConnectedBubblesWithSameLevelController bubblesWithSameLevelController, BubbleData bubbleData,
            IGameStateController gameStateController, SignalBus signalBus)
        {
            _gridMap = gridMap;
            _bubbleData = bubbleData;
            _gameStateController = gameStateController;
            _signalBus = signalBus;
            bubblesWithSameLevelController.BubblesToCombine.Where(x => x != null && x.Count > 1).Subscribe(CollapseBubbles);
        }

        private void CollapseBubbles(List<IBubble> bubbles)
        {
            var level = bubbles[0].Level.Value;
            var levelAfterCombination = GetLevelAfterBubblesCombination(level, bubbles.Count);

            var bubbleWithMaxNeighboursWithResultLevel = FindBubbleToCollapseTo(bubbles, levelAfterCombination, out var toCollapseNeighboursAfterThisCollapse);

            var collapseDuration = _bubbleData.CombiningDuration;

            foreach (var bubble in bubbles)
            {
                _combineBubbleSignal.Bubble = bubble;
                var combinePosition = bubbleWithMaxNeighboursWithResultLevel.Position.Value;
                _combineBubbleSignal.CombinePosition = combinePosition;

                _signalBus.Fire(_combineBubbleSignal);
            }

            var nextState = toCollapseNeighboursAfterThisCollapse > 1 ? GamePlayState.BubblesCombining : GamePlayState.Idle;
            DOVirtual.DelayedCall(collapseDuration, () =>
            {
                foreach (var bubble in bubbles)
                {
                    if (bubble.Position.Value == bubbleWithMaxNeighboursWithResultLevel.Position.Value)
                    {
                        continue;
                    }

                    bubble.Destroy();
                }

                _gameStateController.ChangeGamePlayState(nextState);
            });
        }

        private IBubble FindBubbleToCollapseTo(List<IBubble> bubbles, int level, out int nextNeighbours)
        {
            var maxNumberOfConnections = 1;
            IBubble bubbleToCollapseTo = null;

            foreach (var bubble in bubbles)
            {
                _bubblesToCollapseBufferList.Clear();
                var numberOfConnectionsAfterCollapse =
                    _gridMap.FindBubblesToCollapse(level, bubble.Position.Value, _bubblesToCollapseBufferList).Count;
                if (ShouldUpdateBetterFitBubble(numberOfConnectionsAfterCollapse, maxNumberOfConnections, bubble, bubbleToCollapseTo))
                {
                    maxNumberOfConnections = numberOfConnectionsAfterCollapse;
                    bubbleToCollapseTo = bubble;
                }
            }

            if (bubbleToCollapseTo == null)
            {
                var bubbleAtTop = bubbles.Aggregate((x1, x2) => x1.Position.Value.y > x2.Position.Value.y ? x1 : x2);
                bubbleToCollapseTo = bubbleAtTop;
            }

            nextNeighbours = maxNumberOfConnections;
            return bubbleToCollapseTo;
        }

        private static bool ShouldUpdateBetterFitBubble(int numberOfConnectionsAfterCollapse, int maxNumberOfConnections, IBubble bubble,
            IBubble bubbleToCollapseTo)
        {
            return numberOfConnectionsAfterCollapse > maxNumberOfConnections
                   || (numberOfConnectionsAfterCollapse == maxNumberOfConnections &&
                       (bubbleToCollapseTo == null || bubble.Position.Value.y > bubbleToCollapseTo.Position.Value.y));
        }

        private int GetLevelAfterBubblesCombination(int level, int bubblesCount)
        {
            var result = level + (bubblesCount - 1);
            return result;
        }
    }
}