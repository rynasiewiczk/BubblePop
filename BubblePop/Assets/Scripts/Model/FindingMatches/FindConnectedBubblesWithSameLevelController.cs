using System.Collections.Generic;
using DG.Tweening;
using Enums;
using Project.Pieces;
using Project.Grid;
using UniRx;
using UnityEngine;

namespace Model.FindingMatches
{
    public class FindConnectedBubblesWithSameLevelController : IFindConnectedBubblesWithSameLevelController
    {
        private readonly IGridMap _gridMap = null;
        private readonly IGameStateController _gameStateController = null;
        private readonly PiecesData _piecesData = null;
        public ReactiveProperty<List<IBubble>> BubblesToCombine { get; private set; } = new ReactiveProperty<List<IBubble>>();

        public FindConnectedBubblesWithSameLevelController(IGridMap gridMap, IGameStateController gameStateController, IBubblesSpawner bubblesSpawner,
            PiecesData piecesData)
        {
            _gridMap = gridMap;
            _gameStateController = gameStateController;
            _piecesData = piecesData;

            gameStateController.GamePlayState.Where(x => x == GamePlayState.BubblesCombining)
                .Subscribe(x => CombineBubbles(bubblesSpawner.LatestSpawnedBubble.Value));
        }

        private void CombineBubbles(IBubble bubble)
        {
            DOVirtual.DelayedCall(_piecesData.AfterCombiningDelay, () =>
            {
                var list = new List<IBubble>();
                list = _gridMap.FindBubblesToCollapse(bubble, list);

                if (list.Count == 1)
                {
                    _gameStateController.ChangeGamePlayState(GamePlayState.ScrollRows);
                }
                else if (list.Count > 1)
                {
                    BubblesToCombine.Value = list;
                }
                else
                {
                    Debug.LogError("This should never happen.");
                }
            });
        }
    }
}