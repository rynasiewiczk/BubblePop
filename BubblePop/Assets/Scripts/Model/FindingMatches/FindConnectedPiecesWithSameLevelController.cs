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
        public ReactiveProperty<List<IBubble>> PiecesToCombine { get; private set; } = new ReactiveProperty<List<IBubble>>();

        public FindConnectedPiecesWithSameLevelController(IGridMap gridMap, IGameStateController gameStateController, IBubblesSpawner bubblesSpawner)
        {
            _gridMap = gridMap;
            _gameStateController = gameStateController;

            gameStateController.GamePlayState.Where(x => x == GamePlayState.BubblesCombining)
                .Subscribe(x => CombineBubbles(bubblesSpawner.LatestSpawnedBubble.Value));
        }

        private void CombineBubbles(IBubble bubble)
        {
            var list = new List<IBubble>();
            list = _gridMap.FindBubblesToCollapse(bubble, list);

            if (list.Count == 1)
            {
                _gameStateController.ChangeGamePlayState(GamePlayState.ScrollRows);
            }
            else if (list.Count > 1)
            {
                PiecesToCombine.Value = list;
            }
            else
            {
                Debug.LogError("This should never happen.");
            }
        }
    }
}