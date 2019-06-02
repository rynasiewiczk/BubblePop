using Enums;
using UniRx;

namespace Model
{
    public interface IGameStateController
    {
        ReactiveProperty<GamePlayState> GamePlayState { get; }
        GamePlayState PreviousGamePlayState { get; }

        void ChangeGamePlayState(GamePlayState newState);

        bool Paused { get; }
        void SetPause(bool flag);
    }
}