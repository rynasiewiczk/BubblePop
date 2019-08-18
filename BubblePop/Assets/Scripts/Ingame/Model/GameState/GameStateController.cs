using Enums;
using Ingame.Cheats;
using OutGame;
using UniRx;
using UnityEngine.SceneManagement;
using Zenject;

namespace Model
{
    public class GameStateController : IGameStateController
    {
        private readonly SignalBus _signalBus = null;

        public ReactiveProperty<GamePlayState> GamePlayState { get; private set; }
        public GamePlayState PreviousGamePlayState { get; private set; }

        private readonly IngamePausedSignal _ingamePauseSignal = new IngamePausedSignal();

        public bool Paused { get; private set; } = true;

        public GameStateController(IIngameSceneVisibilityController ingameSceneVisibilityController, SignalBus signalBus)
        {
            _signalBus = signalBus;

            GamePlayState = new ReactiveProperty<GamePlayState>(Enums.GamePlayState.Idle);
            PreviousGamePlayState = Enums.GamePlayState.None;

            ingameSceneVisibilityController.OnIngameUnpaused.Subscribe(x => { Paused = false; });
            
            if (EditorCheats.GameStartedFromIngame())
            {
                Paused = false;
            }
        }

        public void SetPause(bool flag)
        {
            Paused = true;

            _ingamePauseSignal.Paused = true;
            _signalBus.Fire(_ingamePauseSignal);
        }

        public void ChangeGamePlayState(GamePlayState newState)
        {
            PreviousGamePlayState = GamePlayState.Value;
            GamePlayState.Value = newState;
        }
    }
}