using Enums;
using OutGame;
using UniRx;
using Zenject;

namespace Model
{
    public class GameStateController : IGameStateController
    {
        private readonly SignalBus _signalBus = null;

        //TODO: having both properties and signal is duplication of logic. Only signal should remain.
        public ReactiveProperty<GamePlayState> GamePlayState { get; private set; }
        public GamePlayState PreviousGamePlayState { get; private set; }

        private readonly GameStateChangeSignal _gameStateChangeSignal = new GameStateChangeSignal();

        public GameStateController(IIngameSceneVisibilityController ingameSceneVisibilityController, SignalBus signalBus)
        {
            _signalBus = signalBus;

            GamePlayState = new ReactiveProperty<GamePlayState>(Enums.GamePlayState.Paused);
            PreviousGamePlayState = Enums.GamePlayState.None;

            ingameSceneVisibilityController.OnIngameUnpaused.Subscribe(x =>
            {
                var nextState = PreviousGamePlayState == Enums.GamePlayState.None ? Enums.GamePlayState.Idle : PreviousGamePlayState;
                ChangeGamePlayState(nextState);
            });
        }

        public void ChangeGamePlayState(GamePlayState newState)
        {
            PreviousGamePlayState = GamePlayState.Value;
            GamePlayState.Value = newState;

            _gameStateChangeSignal.GamePlayState = newState;
            _gameStateChangeSignal.PrevState = PreviousGamePlayState;
            _signalBus.Fire(_gameStateChangeSignal);
        }
    }
}