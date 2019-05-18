using System.Collections.Generic;
using DG.Tweening;
using Enums;
using Project.Bubbles;
using Project.Grid;
using Sirenix.Utilities;
using UniRx;
using UnityEngine;

namespace Model.FindingMatches
{
    public class FindConnectedBubblesWithSameLevelController : IFindConnectedBubblesWithSameLevelController
    {
        private readonly IGridMap _gridMap = null;
        private readonly IGameStateController _gameStateController = null;
        public ReactiveProperty<List<IBubble>> BubblesToCombine { get; private set; } = new ReactiveProperty<List<IBubble>>();

        public FindConnectedBubblesWithSameLevelController(IGridMap gridMap, IGameStateController gameStateController, IBubblesSpawner bubblesSpawner)
        {
            _gridMap = gridMap;
            _gameStateController = gameStateController;

//            bubblesSpawner.JustSpawned.Where(x => x != null && gameStateController.GamePlayState.Value == GamePlayState.BubblesCombining)
//                .Subscribe(x => CombineBubbles(x));
            gameStateController.GamePlayState.Where(x => x == GamePlayState.BubblesCombining).Subscribe(x => CombineBubbles(bubblesSpawner.JustSpawned.Value));
        }

        private void CombineBubbles(IBubble bubble)
        {
            DOVirtual.DelayedCall(.1f, () =>
            {
                var list = new List<IBubble>();
                list = _gridMap.FindBubblesToCollapse(bubble, list);

                if (list.Count < 1)
                {
                    Debug.LogError("should never happen.");
                }
                else if (list.Count == 1)
                {
                    Debug.Log("Combination not found");
                    _gameStateController.ChangeGamePlayState(GamePlayState.Idle);
                }
                else
                {
                    BubblesToCombine.Value = list;
                }
            });
        }
    }
}