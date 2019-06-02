using Enums;
using Model;
using Project.Grid;
using Project.Input;
using UniRx;
using UnityEngine;

namespace Project.Aiming
{
    public class StartAimingStateObserver : IStartAimingStateObserver
    {
        public StartAimingStateObserver(IInputEventsNotifier inputEventsNotifier, IGameStateController gameStateController, IGridMap gridMap)
        {
            inputEventsNotifier.OnInputStart.Skip(1).Where(inputPosition =>
                    CanStartAiming(gameStateController, gridMap, inputPosition))
                .Subscribe(x => gameStateController.ChangeGamePlayState(GamePlayState.Aiming));
        }

        private static bool CanStartAiming(IGameStateController gameStateController, IGridMap gridMap, Vector2 inputPosition)
        {
            if (inputPosition.y > gridMap.GetViewPosition(gridMap.Size.Value).y)
            {
                return false;
            }

            if (gameStateController.GamePlayState.Value != GamePlayState.Idle)
            {
                return false;
            }

            if (gameStateController.Paused)
            {
                return false;
            }

            return true;
        }
    }
}