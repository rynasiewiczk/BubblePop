using Enums;
using UniRx;

namespace Model
{
    public class GameStateController : IGameStateController
    {
        public ReactiveProperty<GamePlayState> GamePlayState { get; private set; }
        public GamePlayState PreviousGamePlayState { get; private set; }

        public GameStateController()
        {
            GamePlayState = new ReactiveProperty<GamePlayState>(Enums.GamePlayState.Idle);
            PreviousGamePlayState = Enums.GamePlayState.None;
        }

        public void ChangeGamePlayState(GamePlayState newState)
        {
            PreviousGamePlayState = GamePlayState.Value;
            GamePlayState.Value = newState;
        }
    }
}