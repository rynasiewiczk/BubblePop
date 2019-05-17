using System;
using System.Linq;
using Enums;
using Model;
using Project.Grid;
using UniRx;
using UnityEngine;

namespace DefaultNamespace
{
    public class EndAimingStateObserver : IEndAimingStateObserver
    {
        private readonly IBubbleDestinationFinder _bubbleDestinationFinder = null;
        private readonly IGameStateController _gameStateController = null;
        private IGridMap _gridMap = null;

        public EndAimingStateObserver(IInputEventsNotifier inputEventsNotifier, IBubbleDestinationFinder bubbleDestinationFinder,
            IGameStateController gameStateController, IGridMap gridMap)
        {
            _bubbleDestinationFinder = bubbleDestinationFinder;
            _gameStateController = gameStateController;
            _gridMap = gridMap;

            inputEventsNotifier.OnInputEnd.Where(x => _gameStateController.GamePlayState.Value == GamePlayState.Aiming)
                .Subscribe(x => FireBubbleIfFound());
        }

        private void FireBubbleIfFound()
        {
            if (_bubbleDestinationFinder.BubbleAimedData != null)
            {
                var positionToSpawnBubble = GetPositionToSpawnBubble(_bubbleDestinationFinder.BubbleAimedData);
                var cellToSpawnBubble = _gridMap.CellsRegistry.FirstOrDefault(x => x.Position == positionToSpawnBubble);
                if (cellToSpawnBubble == null)
                {
                    Debug.LogError("Trying to spawn bubble outside of grid. Ignoring the try. Provided position: " + positionToSpawnBubble);
                    _gameStateController.GamePlayState.Value = GamePlayState.Idle;
                }
                else if (_gridMap.BubblesRegistry.FirstOrDefault(x => x.Position.Value == positionToSpawnBubble) != null)
                {
                    Debug.LogError("Trying to spawn bubble on cell that already has bubble. Ignoring the try. Provided position: " + positionToSpawnBubble);
                    _gameStateController.GamePlayState.Value = GamePlayState.Idle;
                }
                else
                {
                    //todo: it should change to bubble flying state
                    _gameStateController.GamePlayState.Value = GamePlayState.Idle;
                }
            }
            else
            {
                _gameStateController.GamePlayState.Value = GamePlayState.Idle;
            }
        }

        private Vector2Int GetPositionToSpawnBubble(BubbleAimedData bubbleAimedData)
        {
            var position = bubbleAimedData.Bubble.Position.Value;
            Vector2Int direction;

            var rowSideOfHit = _gridMap.GridRowSidesMap[position.y];

            switch (bubbleAimedData.AimedSide)
            {
                case BubbleSide.TopLeft:
                    direction = new Vector2Int(-1, 0);
                    break;
                case BubbleSide.TopRight:
                    direction = new Vector2Int(1, 0);
                    break;
                case BubbleSide.BottomLeft:
                    direction = rowSideOfHit == GridRowSide.Left ? new Vector2Int(-1, -1) : new Vector2Int(0, -1);
                    break;
                case BubbleSide.BottomRight:
                    direction = rowSideOfHit == GridRowSide.Left ? new Vector2Int(0, -1) : new Vector2Int(1, -1);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var result = position + direction;

            Debug.DrawLine((Vector2) result, (Vector2) result + Vector2.down / 2, Color.red);
            Debug.DrawLine((Vector2) result, (Vector2) result + Vector2.left / 2, Color.red);
            Debug.DrawLine((Vector2) result, (Vector2) result + Vector2.right / 2, Color.red);
            Debug.DrawLine((Vector2) result, (Vector2) result + Vector2.up / 2, Color.red);

            return result;
        }
    }
}