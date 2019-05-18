using System.Collections.Generic;
using Enums;
using Project.Bubbles;
using Project.Grid;
using UniRx;
using UnityEngine;

namespace Model.FindingMatches
{
    public class FindConnectedBubblesWithSameLevel : IFindConnectedBubblesWithSameLevel
    {
        private readonly IGridMap _gridMap = null;
        private readonly IGameStateController _gameStateController = null;
        private readonly IBubblesSpawner _bubblesSpawner = null;

        private List<IBubble> _bubblesToCombineBufferList = new List<IBubble>(10);

        public FindConnectedBubblesWithSameLevel(IGridMap gridMap, IGameStateController gameStateController, IBubblesSpawner bubblesSpawner)
        {
            _gridMap = gridMap;
            _gameStateController = gameStateController;
            _bubblesSpawner = bubblesSpawner;

            gameStateController.GamePlayState.Where(x => x == GamePlayState.BubblesCombining).Subscribe(x => CombineBubbles(_bubblesSpawner.JustSpawned.Value));
        }

        private void CombineBubbles(IBubble bubble)
        {
            _bubblesToCombineBufferList.Clear();
            _bubblesToCombineBufferList = _gridMap.FindBubblesToCollapse(bubble, _bubblesToCombineBufferList);

            if (_bubblesToCombineBufferList.Count < 1)
            {
                Debug.LogError("should never happen.");
            }
            else if (_bubblesToCombineBufferList.Count == 1)
            {
                Debug.Log("Combination not found");
                _gameStateController.ChangeGamePlayState(GamePlayState.Idle);
            }
            else
            {
                _gameStateController.ChangeGamePlayState(GamePlayState.Idle);
            }
        }
    }
}