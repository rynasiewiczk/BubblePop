using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Enums;
using Model.FindingMatches;
using Project.Bubbles;
using Project.Grid;
using UniRx;
using UnityEngine;
using View;
using Zenject;

namespace Model.CombiningBubbles
{
    public class CombineBubbles : ICombineBubbles
    {
        private readonly IGridMap _gridMap = null;
        private readonly List<IBubble> _bubblesToCollapseBufferList = new List<IBubble>(10);
        private BubbleData _bubbleData = null;
        private CombineBubbleSignal _combineBubbleSignal = new CombineBubbleSignal();
        private SpawnBubbleOnGridSignal _spawnBubbleOnGridSignal = new SpawnBubbleOnGridSignal();
        private SignalBus _signalBus = null;
        private IGameStateController _gameStateController = null;
        public int LastCombinedBubbleNeighboursWithSameLevelAmount { get; private set; }
        

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
            _gameStateController.ChangeGamePlayState(GamePlayState.WaitingForBubblesCombine);

            var level = bubbles[0].Level.Value;
            var levelAfterCombination = GetLevelAfterBubblesCombination(level, bubbles.Count);

            var bubbleWithMaxNeighboursWithResultLevel = FindBubbleToCollapseTo(bubbles, levelAfterCombination, out var toCollapseNeighboursAfterThisCollapse);
            LastCombinedBubbleNeighboursWithSameLevelAmount = toCollapseNeighboursAfterThisCollapse;
            
            var positionOfCollapse = bubbleWithMaxNeighboursWithResultLevel.Position.Value;

            var collapseDuration = _bubbleData.CombiningDuration;

            foreach (var bubble in bubbles)
            {
                _combineBubbleSignal.Bubble = bubble;
                var combinePosition = bubbleWithMaxNeighboursWithResultLevel.Position.Value;
                _combineBubbleSignal.CombinePosition = combinePosition;

                _signalBus.Fire(_combineBubbleSignal);
            }

            DOVirtual.DelayedCall(collapseDuration, () =>
            {
                foreach (var bubble in bubbles)
                {
                    if (bubble.IsPlayable())
                    {
                        bubble.Destroy();
                    }
                }

                _spawnBubbleOnGridSignal.Level = levelAfterCombination;
                _spawnBubbleOnGridSignal.Position = positionOfCollapse;
                _signalBus.Fire(_spawnBubbleOnGridSignal);


                _gameStateController.ChangeGamePlayState(GamePlayState.DropBubblesAfterCombining);
            });
        }

        private IBubble FindBubbleToCollapseTo(List<IBubble> bubbles, int level, out int nextNeighbours)
        {
            var maxNumberOfConnections = 1;
            IBubble bubbleToCollapseTo = null;

            foreach (var bubble in bubbles)
            {
                _bubblesToCollapseBufferList.Clear();
                var bubblesToCollect = _gridMap.FindBubblesToCollapse(level, bubble.Position.Value, _bubblesToCollapseBufferList);
                int bubblesToCollapseCount = bubblesToCollect.Count;
                if (ShouldUpdateBetterFitBubble(bubblesToCollapseCount, maxNumberOfConnections, bubble, bubbleToCollapseTo))
                {
                    maxNumberOfConnections = bubblesToCollapseCount;
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