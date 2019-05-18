using Enums;
using UniRx;
using UnityEngine;

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

            GamePlayState.Subscribe(x => Debug.Log(x));
        }

        public void ChangeGamePlayState(GamePlayState newState)
        {
            PreviousGamePlayState = GamePlayState.Value;
            GamePlayState.Value = newState;
        }
    }
}