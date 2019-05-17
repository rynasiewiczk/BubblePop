using Enums;
using Model;
using UniRx;

namespace DefaultNamespace
{
    public class StartAimingStateObserver : IStartAimingStateObserver
    {
        public StartAimingStateObserver(IInputEventsNotifier inputEventsNotifier, IGameStateController gameStateController)
        {
            inputEventsNotifier.OnInputStart.Skip(1).Where(x => gameStateController.GamePlayState.Value == GamePlayState.Idle)
                .Subscribe(x => gameStateController.ChangeGamePlayState(GamePlayState.Aiming));
        }
    }
}