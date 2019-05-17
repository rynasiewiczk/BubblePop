using System.Linq;
using Enums;
using Model;
using Project.Grid;
using Project.Input;
using UniRx;
using UnityEngine;

namespace Project.Aiming
{
    public class EndAimingStateObserver : IEndAimingStateObserver
    {
        private readonly IBubbleDestinationFinder _bubbleDestinationFinder = null;
        private readonly IGameStateController _gameStateController = null;
        private readonly IGridMap _gridMap = null;

        public ReactiveProperty<Vector2[]> BubbleFlyPath { get; }

        public EndAimingStateObserver(IInputEventsNotifier inputEventsNotifier, IBubbleDestinationFinder bubbleDestinationFinder,
            IGameStateController gameStateController, IGridMap gridMap)
        {
            BubbleFlyPath = new ReactiveProperty<Vector2[]>();

            _bubbleDestinationFinder = bubbleDestinationFinder;
            _gameStateController = gameStateController;
            _gridMap = gridMap;

            inputEventsNotifier.OnInputEnd.Where(x => _gameStateController.GamePlayState.Value == GamePlayState.Aiming)
                .Subscribe(x => GetPlaceToSpawnBubble());
        }

        private void GetPlaceToSpawnBubble()
        {
            if (_bubbleDestinationFinder.BubbleAimedData != null)
            {
                var positionToSpawnBubble = _gridMap.GetPositionToSpawnBubble(_bubbleDestinationFinder.BubbleAimedData.Bubble,
                    _bubbleDestinationFinder.BubbleAimedData.AimedSide);
                var cellToSpawnBubble = _gridMap.CellsRegistry.FirstOrDefault(x => x.Position == positionToSpawnBubble);
                if (cellToSpawnBubble == null)
                {
                    Debug.LogError("Trying to spawn bubble outside of grid. Ignoring the try. Provided position: " + positionToSpawnBubble);
                    _gameStateController.ChangeGamePlayState(GamePlayState.Idle);
                }
                else if (_gridMap.BubblesRegistry.FirstOrDefault(x => x.Position.Value == positionToSpawnBubble) != null)
                {
                    Debug.LogError("Trying to spawn bubble on cell that already has bubble. Ignoring the try. Provided position: " + positionToSpawnBubble);
                    _gameStateController.ChangeGamePlayState(GamePlayState.Idle);
                }
                else
                {
                    BubbleFlyPath.Value = _bubbleDestinationFinder.BubbleAimedData.Path;
                }
            }
            else
            {
                _gameStateController.ChangeGamePlayState(GamePlayState.Idle);
            }
        }
    }
}